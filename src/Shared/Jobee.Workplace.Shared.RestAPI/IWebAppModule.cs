using Jobee.Workplace.Shared.Infrastructure.Modules;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Jobee.Workplace.Shared.RestAPI;

public interface IWebAppModule : IAppModule
{
    void SwaggerUIAction(SwaggerUIOptions options);

    public void SwaggerGenAction(SwaggerGenOptions options);

    void RegisterEndpoints(WebApplication app);
}