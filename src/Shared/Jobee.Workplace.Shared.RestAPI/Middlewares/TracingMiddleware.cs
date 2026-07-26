using Jobee.Workplace.Shared.Application.Tracing;

namespace Jobee.Workplace.Shared.RestAPI.Middlewares;

internal sealed class TracingMiddleware : IMiddleware
{
    private readonly IOperationContextSetter _operationContextSetter;

    public TracingMiddleware(IOperationContextSetter operationContextSetter) =>
        _operationContextSetter = operationContextSetter;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId)
            || string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.CreateVersion7().ToString();
        }

        _ = context.Response.Headers.TryAdd("X-Correlation-ID", correlationId);

        var operationContext = new OperationContext(correlationId!);
        _operationContextSetter.OperationContext = operationContext;

        await next(context);
    }
}