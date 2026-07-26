using System.Diagnostics;
using Jobee.Workplace.Shared.Application.Exceptions;
using Jobee.Workplace.Shared.Contracts.Errors;
using Jobee.Workplace.Shared.RestAPI.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Jobee.Workplace.Shared.RestAPI.Exceptions;

internal sealed class ConflictExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictException conflictException)
        {
            return false;
        }

        var currentActivity = Activity.Current;
        var traceId = currentActivity?.TraceId.ToString() ?? httpContext.TraceIdentifier;
        var response = new ValidationErrorResponse(httpContext.Request.Path,
            traceId,
            [MemberError.Conflict(conflictException.Reference)]);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}