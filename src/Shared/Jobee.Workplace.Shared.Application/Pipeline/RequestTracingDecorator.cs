using System.Diagnostics;
using Jobee.Workplace.Shared.Application.Tracing;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal sealed class RequestTracingDecorator<TRequest, TResponse>
{
    private readonly ILogger<RequestTracingDecorator<TRequest, TResponse>> _logger;
    private readonly IOperationContextAccessor _operationContextAccessor;

    public RequestTracingDecorator(IOperationContextAccessor operationContextAccessor,
        ILogger<RequestTracingDecorator<TRequest, TResponse>> logger)
    {
        _operationContextAccessor = operationContextAccessor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        _logger.OperationStarted(requestType.Name);

        var operationContext = _operationContextAccessor.OperationContext;

        using var activity = TracingSources.Default.CreateActivity(requestType.Name, ActivityKind.Internal);
        activity?.Start();

        operationContext.OperationName = requestType.Name;

        var result = await next(cancellationToken);

        _logger.OperationFinished(requestType.Name);

        return result;
    }
}