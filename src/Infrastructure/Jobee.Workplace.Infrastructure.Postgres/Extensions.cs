using System.Text.Json;
using System.Text.Json.Serialization;
using Jobee.Workplace.Shared.Infrastructure;
using Jobee.Workplace.Shared.Infrastructure.DataAccess;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Jobee.Workplace.Infrastructure.Postgres;

public static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection serviceCollection)
    {
        var assemblyCollection = InfrastructureAssemblyCollection.LoadFromCurrentDomain();
        
        serviceCollection.AddSingleton(assemblyCollection);
        serviceCollection.AddDbContextFactory<DbContext>((serviceProvider, optionsBuilder) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("Postgres");
                var converters = serviceProvider.GetRequiredService<IEnumerable<JsonConverter>>();

                var serializationOptions = new JsonSerializerOptions();
                foreach (var converter in converters)
                {
                    serializationOptions.Converters.Add(converter);
                }

                var builder = new NpgsqlDataSourceBuilder(connectionString)
                    .ConfigureTracing(t =>
                    {
                    })
                    .EnableDynamicJson()
                    .ConfigureJsonOptions(serializationOptions);

                optionsBuilder.UseNpgsql(builder.Build());
                optionsBuilder.AddInterceptors(new EventPublisherInterceptor());
                optionsBuilder.UseApplicationServiceProvider(serviceProvider);
            }, ServiceLifetime.Scoped)
            .AddScoped<DbContext>(sp =>
            {
                var factory = sp.GetRequiredService<IDbContextFactory<DbContext>>();
                return factory.CreateDbContext();
            })
            .AddDataProtection()
            .PersistKeysToDbContext<PostgresDbContext>();

        serviceCollection.AddHealthChecks()
            .AddDbContextCheck<PostgresDbContext>(tags: ["live", "ready"]);
        
        return serviceCollection;
    }
}