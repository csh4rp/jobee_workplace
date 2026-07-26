using System.Collections;
using System.Reflection;

namespace Jobee.Workplace.Shared.Infrastructure;

public class InfrastructureAssemblyCollection : IEnumerable<Assembly>
{
    private readonly List<Assembly> _assemblies;
    
    public InfrastructureAssemblyCollection(List<Assembly> assemblies)
    {
        _assemblies = assemblies;
    }

    public IEnumerator<Assembly> GetEnumerator() => _assemblies.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public static InfrastructureAssemblyCollection LoadFromCurrentDomain()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.Contains("Infrastructure") is true)
            .ToList();
        
        return new InfrastructureAssemblyCollection(assemblies);
    }
}