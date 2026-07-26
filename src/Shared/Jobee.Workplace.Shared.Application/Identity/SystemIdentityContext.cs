namespace Jobee.Workplace.Shared.Application.Identity;

public sealed class SystemIdentityContext : IIdentityContext
{
    private readonly IReadOnlyList<Permission> _permissions;

    public SystemIdentityContext(IReadOnlyList<Permission> permissions) => _permissions = permissions;

    public string Subject => "SYSTEM";

    public string App => "UNKNOWN";

    public bool HasAccessTo(string permission) => _permissions.Any(p => p.GrantsAccessTo(permission));

    public IReadOnlyList<string> GetPermissions() => [.. _permissions.Select(p => p.ToString())];
}