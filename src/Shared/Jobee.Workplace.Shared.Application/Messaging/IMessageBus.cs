using Jobee.Workplace.Shared.Common;

namespace Jobee.Workplace.Shared.Application.Messaging;


public interface IMessageBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent;

    Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken);
}