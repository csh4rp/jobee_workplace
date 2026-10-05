using System.Collections.Frozen;
using System.Reflection;
using Jobee.Workplace.Jobs.Infrastructure;
using Jobee.Workplace.Jobs.RestApi.Categories;
using Jobee.Workplace.Jobs.RestApi.Companies;
using Jobee.Workplace.Shared.RestAPI;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Jobee.Workplace.Jobs.RestApi;

public class JobsModule : IWebAppModule
{
    public string Name => "Jobs";

    public FrozenSet<Assembly> Assemblies { get; } =
    [
        Assembly.Load("Jobee.Workplace.Jobs.Application"),
        Assembly.Load("Jobee.Workplace.Jobs.Domain"),
        Assembly.Load("Jobee.Workplace.Jobs.Contracts"),
        Assembly.Load("Jobee.Workplace.Jobs.Infrastructure"),
        Assembly.Load("Jobee.Workplace.Jobs.RestApi"),
    ];
    
    
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure();
        return services;
    }

    public void SwaggerUIAction(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("/swagger/jobs-v1/swagger.json", "Jobs v1.0");
    }

    public void SwaggerGenAction(SwaggerGenOptions options)
    {
        options.SwaggerDoc("jobs-v1", new OpenApiInfo { Version = "v1.0" });
    }

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapCategoryEndpoints();
        app.MapCompanyEndpoints();
    }
}