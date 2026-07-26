namespace Jobee.Workplace.Shared.Application.Identity;

public sealed class AnonymousIdentityContext : IIdentityContext
{
    private readonly IReadOnlyList<Permission> _permissions;

    public AnonymousIdentityContext(IReadOnlyList<Permission> permissions) => _permissions = permissions;

    public string Subject => "ANONYMOUS";

    public string App => "UNKNOWN";

    public bool HasAccessTo(string permission) => _permissions.Any(p => p.GrantsAccessTo(permission));

    public IReadOnlyList<string> GetPermissions() => [.. _permissions.Select(p => p.ToString())];
}