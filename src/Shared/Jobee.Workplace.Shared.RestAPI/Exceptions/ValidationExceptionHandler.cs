using System.Diagnostics;
using Jobee.Workplace.Shared.Application.Exceptions;
using Jobee.Workplace.Shared.RestAPI.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Jobee.Workplace.Shared.RestAPI.Exceptions;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var currentActivity = Activity.Current;
        var traceId = currentActivity?.TraceId.ToString() ?? httpContext.TraceIdentifier;

        var response = new ValidationErrorResponse(httpContext.Request.Path, traceId, validationException.Errors);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}