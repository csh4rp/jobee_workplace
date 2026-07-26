using Jobee.Workplace.Shared.Application;
using Jobee.Workplace.Shared.Application.Messaging;
using Jobee.Workplace.Shared.Common;
using MassTransit;

namespace Jobee.Workplace.Infrastructure.RabbitMQ;

internal sealed class MessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly TimeProvider _timeProvider;

    public MessageBus(IPublishEndpoint publishEndpoint, TimeProvider timeProvider)
    {
        _publishEndpoint = publishEndpoint;
        _timeProvider = timeProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent
    {
        await _publishEndpoint.Publish(@event, publishContext =>
        {
            // publishContext.Headers.Set(MessagingConstants.TimestampHeaderName, _timeProvider.GetUtcNow());
            // publishContext.Headers.Set(MessagingConstants.SubjectHeaderName, identityContext.Subject);
            publishContext.Headers.Set(MessagingConstants.TimestampHeaderName, _timeProvider.GetUtcNow());
            // publishContext.Headers.Set(MessagingConstants.SubjectHeaderName, identityContext.Subject);
            // publishContext.Headers.Set(MessagingConstants.AppHeaderName, identityContext.App);
            // publishContext.Headers.Set(MessagingConstants.PermissionsHeaderName, identityContext.GetPermissions());
            // publishContext.Headers.Set(MessagingConstants.CorrelationIdHeaderName, operationContext.CorrelationId);
        }, cancellationToken);
    }

    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            var method = GetType().GetMethods()
                .First(m => m is
                {
                    Name: nameof(this.PublishAsync),
                    IsGenericMethod: true
                });

            var genericMethod = method.MakeGenericMethod(eventType);
            await (Task)genericMethod.Invoke(this, [@event, cancellationToken])!;
        }
    }
}