namespace Jobee.Workplace.Shared.Contracts.Errors;

public class ConflictError : Error
{
    public ConflictError(string target, string message)
        : base(ErrorCodes.Conflict, message) => Target = target;

    public string Target { get; }

}