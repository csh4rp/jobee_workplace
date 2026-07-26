namespace Jobee.Workplace.Shared.Application.Tracing;

internal sealed class OperationContextWrapper : IOperationContextAccessor, IOperationContextSetter
{
    public OperationContext OperationContext
    {
        get => field ?? throw new InvalidOperationException("OperationContext was not set");
        set
        {
            if (field is not null)
            {
                throw new InvalidOperationException("OperationContext is already set");
            }

            field = value;
        }
    }
}