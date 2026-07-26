using Jobee.Workplace.Shared.Contracts.Errors;

namespace Jobee.Workplace.Shared.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(params MemberError[] propertyErrors) :
        base("One or more validation errors occurred.") => Errors = propertyErrors;

    public string Code => "VALIDATION_ERROR";

    public IReadOnlyList<MemberError> Errors { get; }
}
