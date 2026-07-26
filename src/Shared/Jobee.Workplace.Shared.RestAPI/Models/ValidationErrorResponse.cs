using System.Diagnostics.CodeAnalysis;
using Jobee.Workplace.Shared.Contracts.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Jobee.Workplace.Shared.RestAPI.Models;

[ExcludeFromCodeCoverage]
public class ValidationErrorResponse : ProblemDetails
{
    public ValidationErrorResponse(string instance, string traceId, IEnumerable<MemberError> errors)
    {
        Status = 400;
        Title = "One or more validation errors occurred";
        Detail = "One or more validation errors occurred";
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
        Instance = instance;
        TraceId = traceId;
        Timestamp = DateTimeOffset.UtcNow;
        Errors = errors as IReadOnlyList<MemberError> ?? [.. errors];
    }

    public string TraceId { get; }

    public DateTimeOffset Timestamp { get; }

    public IReadOnlyList<MemberError> Errors { get; }
}