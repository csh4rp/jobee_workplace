using Microsoft.Extensions.Configuration;

namespace Jobee.Workplace.Shared.Application.Identity;

public sealed class StaticPermissionsProvider
{
    private static readonly List<Permission> UserPermissions = [];
    private static readonly List<Permission> AnonymousPermissions = [];
    private static readonly List<Permission> SystemPermissions = [];

    public StaticPermissionsProvider(IConfiguration configuration)
    {
        var authSection = configuration.GetSection("Auth");

        foreach (var module in authSection.GetChildren())
        {
            var permissionsSection = module.GetSection("StaticPermissions");
            var userSection = permissionsSection.GetSection("User");
            var anonymousSection = permissionsSection.GetSection("Anonymous");
            var systemSection = permissionsSection.GetSection("System");
            if (!permissionsSection.Exists() || !userSection.Exists()
                                             || !anonymousSection.Exists()
                                             || !systemSection.Exists()
               )
            {
                continue;
            }

            var userPermissions = userSection.Get<string[]>()?.Select(Permission.Parse) ?? [];
            var anonymousPermissions = anonymousSection.Get<string[]>()?.Select(Permission.Parse) ?? [];
            var systemPermissions = systemSection.Get<string[]>()?.Select(Permission.Parse) ?? [];

            UserPermissions.AddRange(userPermissions);
            AnonymousPermissions.AddRange(anonymousPermissions);
            SystemPermissions.AddRange(systemPermissions);
        }
    }

    public IReadOnlyList<Permission> GetUserPermissions() => UserPermissions.AsReadOnly();

    public IReadOnlyList<Permission> GetAnonymousPermissions() => AnonymousPermissions.AsReadOnly();

    public IReadOnlyList<Permission> GetSystemPermissions() => SystemPermissions.AsReadOnly();
}
