using Jobee.Workplace.Shared.Application.Messaging;
using Jobee.Workplace.Shared.Application.Tracing;
using MassTransit;
using Serilog.Context;

namespace Jobee.Workplace.Shared.Infrastructure.Messaging;

public sealed class TracingFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly IOperationContextSetter _operationContextSetter;

    public TracingFilter(IOperationContextSetter operationContextSetter) =>
        _operationContextSetter = operationContextSetter;

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        if (!context.TryGetHeader(MessagingConstants.CorrelationIdHeaderName, out string? correlationId)
            || string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.CreateVersion7().ToString();
        }

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            var operationContext = new OperationContext(correlationId);
            _operationContextSetter.OperationContext = operationContext;
            await next.Send(context);
        }
    }

    public void Probe(ProbeContext context) => context.CreateFilterScope("TracingScope");
}