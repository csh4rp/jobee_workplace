namespace Jobee.Workplace.Shared.Application.Tracing;

public interface IOperationContextAccessor
{
    OperationContext OperationContext { get; }
}