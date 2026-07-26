namespace Jobee.Workplace.Shared.Contracts.Errors;

public abstract class ErrorCodes
{
    public const string Required = "REQUIRED";

    public const string InvalidValue = "INVALID_ARGUMENT";

    public const string Conflict = "CONFLICT";

    public const string EntityNotFound = "ENTITY_NOT_FOUND";

    public const string OutOfRange = "OUT_OF_RANGE";

    public const string InputTooLong = "INPUT_TOO_LONG";
}