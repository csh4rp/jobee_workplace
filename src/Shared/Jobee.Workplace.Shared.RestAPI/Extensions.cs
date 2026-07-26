using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using Jobee.Workplace.Shared.Application.Identity;
using Jobee.Workplace.Shared.Application.Pipeline;
using Jobee.Workplace.Shared.Application.Tracing;
using Jobee.Workplace.Shared.Infrastructure;
using Jobee.Workplace.Shared.Infrastructure.Modules;
using Jobee.Workplace.Shared.RestAPI.Exceptions;
using Jobee.Workplace.Shared.RestAPI.Middlewares;
using Jobee.Workplace.Shared.RestAPI.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.OpenApi;

namespace Jobee.Workplace.Shared.RestAPI;

public static class Extensions
{
    public static IEnumerable<IWebAppModule> GetWebAppModules(this IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var location = executingAssembly.Location;
        var currentDirectory = Path.GetDirectoryName(location);

        var files = Directory.GetFiles(currentDirectory!)
            .ToList();

        var modulesSection = configuration.GetRequiredSection("Modules");

        foreach (var moduleSection in modulesSection.GetChildren())
        {
            if (!string.Equals(moduleSection.GetSection("Enabled").Value, bool.TrueString,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            var name = moduleSection.Key;

            var appModuleType = files
                .Where(file => file.EndsWith($".{name}.RestAPI.dll"))
                .Select(Assembly.LoadFile)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .FirstOrDefault(t => t.IsAssignableTo(typeof(IWebAppModule)));

            if (appModuleType is null)
            {
                continue;
            }

            yield return (IWebAppModule)Activator.CreateInstance(appModuleType)!;
        }
    }

    public static WebApplicationBuilder RegisterRestApiModules(this WebApplicationBuilder builder)
    {
        var modules = builder.Configuration.GetWebAppModules().ToList();

        foreach (var module in modules)
        {
            builder.Services.AddSingleton<IAppModule>(_ => module);
        }

        builder.Services.AddApiVersioning(options =>
        {
            options.ApiVersionReader = new MediaTypeApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerWithBearerToken(modules);
        }

        var authBuilder = builder.Services.AddAuthorization()
            .AddAuthentication(o => { o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; });

        foreach (var authSection in builder.Configuration.GetSection("Auth").GetChildren())
        {
            if (authSection.GetValue<string>("Authority") is null)
            {
                continue;
            }

            authBuilder.AddJwtBearer(authSection.Key, opts =>
            {
                opts.Authority = authSection.GetValue<string>("Authority");
                opts.TokenValidationParameters.ValidIssuer = authSection.GetValue<string>("Issuer");
                opts.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
                opts.TokenValidationParameters.ValidateAudience = false;
            });
        }

        return builder;
    }

    public static IServiceCollection AddSwaggerWithBearerToken(this IServiceCollection serviceCollection,
        List<IWebAppModule> modules)
    {
        serviceCollection.AddEndpointsApiExplorer()
            .AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                opt.AddSecurityRequirement(_ =>
                {
                    var requirement = new OpenApiSecurityRequirement();
                    var scheme = new OpenApiSecuritySchemeReference("Bearer");
                    requirement[scheme] = new List<string>();
                    return requirement;
                });
            });

        return serviceCollection;
    }

    private static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        var modules = builder.Configuration.GetAppModules().ToList();

        foreach (var module in modules)
        {
            builder.Services.AddSingleton<IAppModule>(_ => (IAppModule)Activator.CreateInstance(module.GetType())!);
        }

        var assemblies = modules.SelectMany(m => m.Assemblies).ToArray();

        builder.Services.Configure<BasicAuthOptions>(builder.Configuration.GetSection("Auth:Basic"));

        builder.Services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true)
            .AddRequestPipeline(assemblies)
            .AddIdentityContext()
            .AddHttpContextAccessor()
            .AddExceptionHandlers()
            .AddSingleton(TimeProvider.System)
            .AddTracingServices()
            .AddScoped<TracingMiddleware>()
            .AddScoped<IdentityMiddleware>()
            .AddScoped<BasicAuthMiddleware>()
            .AddDistributedMemoryCache()
            .AddMemoryCache()
            .AddHybridCache(opt =>
            {
                opt.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Flags = HybridCacheEntryFlags.None,
                    LocalCacheExpiration = TimeSpan.FromSeconds(30),
                    Expiration = TimeSpan.FromMinutes(5)
                };
            });


        foreach (var module in modules)
        {
            module.RegisterServices(builder.Services, builder.Configuration.GetSection($"Modules:{module.Name}"));
        }

        return builder;
    }
    
    public static WebApplication PrepareRestApiPipeline(this WebApplication app)
    {
        app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var modules = app.Services.GetServices<IWebAppModule>().ToList();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(o =>
            {
            });

            app.UseSwaggerUI(options =>
            {
                foreach (var module in modules)
                {
                    module.SwaggerUIAction(options);
                }
            });
        }

        app.UseExceptionHandler(o =>
        {
            o.Run(async cnx =>
            {
                await Task.CompletedTask;
            });
        });

        app.UseMiddleware<BasicAuthMiddleware>();
        app.MapPrometheusScrapingEndpoint();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<TracingMiddleware>();
        app.UseMiddleware<IdentityMiddleware>();
        app.UseEndpoints(e =>
        {
        });

        foreach (var module in modules)
        {
            module.RegisterEndpoints(app);
        }

        app.MapHealthChecks("/health/_live", new HealthCheckOptions { Predicate = p => p.Tags.Contains("live") });
        app.MapHealthChecks("/health/_ready", new HealthCheckOptions { Predicate = p => p.Tags.Contains("ready") });

        return app;
    }

}