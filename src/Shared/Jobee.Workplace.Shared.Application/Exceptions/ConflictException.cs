namespace Jobee.Workplace.Shared.Application.Exceptions;

public class ConflictException : Exception
{
    public required string Reference { get; set; }
}