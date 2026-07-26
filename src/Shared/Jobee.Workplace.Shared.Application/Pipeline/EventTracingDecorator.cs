using System.Diagnostics;
using Jobee.Workplace.Shared.Application.Tracing;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal sealed class EventTracingDecorator<TEvent>: INotificationHandler<TEvent> where TEvent : INotification
{
    private readonly ILogger<EventTracingDecorator<TEvent>> _logger;
    private readonly INotificationHandler<TEvent> _notificationHandler;
    private readonly IOperationContextAccessor _operationContextAccessor;

    public EventTracingDecorator(INotificationHandler<TEvent> notificationHandler,
        IOperationContextAccessor operationContextAccessor, ILogger<EventTracingDecorator<TEvent>> logger)
    {
        _notificationHandler = notificationHandler;
        _operationContextAccessor = operationContextAccessor;
        _logger = logger;
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var requestType = typeof(TEvent);
        _logger.OperationStarted(requestType.Name);

        var operationContext = _operationContextAccessor.OperationContext;

        using var activity = TracingSources.Default.CreateActivity(requestType.Name, ActivityKind.Internal);
        activity?.Start();

        operationContext.OperationName = requestType.Name;

        await _notificationHandler.Handle(notification, cancellationToken);

        _logger.OperationFinished(requestType.Name);
    }
}