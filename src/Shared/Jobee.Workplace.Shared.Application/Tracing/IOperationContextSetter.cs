namespace Jobee.Workplace.Shared.Application.Tracing;

public interface IOperationContextSetter
{
    OperationContext OperationContext { set; }
}