using System.Collections.Frozen;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Shared.Infrastructure.Modules;

public interface IAppModule
{
    string Name { get; }
    
    public FrozenSet<Assembly> Assemblies { get; }
    
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration);
}