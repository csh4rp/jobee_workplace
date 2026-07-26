namespace Jobee.Workplace.Shared.Application.Identity;

public interface IIdentityContext
{
    string Subject { get; }

    string App { get; }

    bool HasAccessTo(string permission);

    IReadOnlyList<string> GetPermissions();
}