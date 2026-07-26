namespace Jobee.Workplace.Shared.Application.Identity;

internal sealed class IdentityContextWrapper : IIdentityContextSetter, IIdentityContextAccessor
{
    public IIdentityContext IdentityContext
    {
        get => field ?? throw new InvalidOperationException("IdentityContext is not set");
        set
        {
            if (field is not null)
            {
                throw new InvalidOperationException("IdentityContext is already set");
            }

            field = value;
        }
    }
}