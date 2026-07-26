using System.Reflection;
using Jobee.Workplace.Shared.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;

namespace Jobee.Workplace.Shared.Infrastructure;

public static class Extensions
{
    public static IEnumerable<IAppModule> GetAppModules(this IConfiguration configuration)
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
                .FirstOrDefault(t => t.IsAssignableTo(typeof(IAppModule)));

            if (appModuleType is null)
            {
                continue;
            }

            yield return (IAppModule)Activator.CreateInstance(appModuleType)!;
        }
    }
}