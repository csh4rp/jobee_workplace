namespace Jobee.Workplace.Shared.Application.Identity;

public sealed class UserIdentityContext : IIdentityContext
{
    private readonly IReadOnlyList<Permission> _permissions;

    public UserIdentityContext(Guid userId, string app, IEnumerable<Permission> permissions)
    {
        _permissions = permissions as IReadOnlyList<Permission> ?? permissions.ToList();
        Subject = userId.ToString();
        App = app;
    }

    public string Subject { get; }

    public string App { get; }

    public bool HasAccessTo(string permission) => _permissions.Any(p => p.GrantsAccessTo(permission));

    public IReadOnlyList<string> GetPermissions() => [.. _permissions.Select(p => p.ToString())];
}