namespace Jobee.Workplace.Shared.Application.Identity;

public interface IIdentityContextAccessor
{
    IIdentityContext IdentityContext { get; }
}