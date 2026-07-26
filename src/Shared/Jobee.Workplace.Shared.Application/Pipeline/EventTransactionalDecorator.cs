using System.Reflection;
using Jobee.Workplace.Shared.Application.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal sealed class EventTransactionalDecorator<TEvent> : INotificationHandler<TEvent> where TEvent : INotification
{
    private readonly ILogger<EventTransactionalDecorator<TEvent>> _logger;
    private readonly INotificationHandler<TEvent> _notificationHandler;
    private readonly IUnitOfWork _unitOfWork;

    public EventTransactionalDecorator(INotificationHandler<TEvent> notificationHandler,
        IUnitOfWork unitOfWork,
        ILogger<EventTransactionalDecorator<TEvent>> logger)
    {
        _notificationHandler = notificationHandler;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var type = typeof(TEvent);

        if (TransactionalTypesDictionary.ShouldUseTransaction(type) == false)
        {
            await _notificationHandler.Handle(notification, cancellationToken);
            return;
        }

        var handlerType = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith("Jobee"))
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsAssignableTo(type));

        var hasTransactionalAttribute = handlerType?.GetCustomAttribute<TransactionalAttribute>();

        var shouldUseTransaction = hasTransactionalAttribute is not null;
        TransactionalTypesDictionary.Add(type, shouldUseTransaction);

        if (!shouldUseTransaction)
        {
            await _notificationHandler.Handle(notification, cancellationToken);
            return;
        }

        await using var scope = await _unitOfWork.BeginScopeAsync(cancellationToken);
        _logger.TransactionStarted(type.Name);

        await _notificationHandler.Handle(notification, cancellationToken);

        await scope.CommitAsync(cancellationToken);
        _logger.TransactionCompleted(type.Name);
    }
}