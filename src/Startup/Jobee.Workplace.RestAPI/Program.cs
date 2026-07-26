using Jobee.Workplace.Shared.RestAPI;

namespace Jobee.Workplace.RestAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args)
            .RegisterRestApiModules();

        var app = builder.Build()
            .PrepareRestApiPipeline();

        await app.RunAsync();
    }
}