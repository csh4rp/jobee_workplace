using Jobee.Workplace.Shared.Application.Identity;
using Jobee.Workplace.Shared.Application.Messaging;
using Jobee.Workplace.Shared.Application.Tracing;
using MassTransit;

namespace Jobee.Workplace.Shared.Infrastructure.Messaging;

public class IdentityFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly IIdentityContextSetter _identityContextSetter;
    private readonly IOperationContextAccessor _operationContextAccessor;
    private readonly StaticPermissionsProvider _staticPermissionsProvider;

    public IdentityFilter(IOperationContextAccessor operationContextAccessor,
        IIdentityContextSetter identityContextSetter, StaticPermissionsProvider staticPermissionsProvider)
    {
        _operationContextAccessor = operationContextAccessor;
        _identityContextSetter = identityContextSetter;
        _staticPermissionsProvider = staticPermissionsProvider;
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        string? subject = null, appName = null;

        if (!context.TryGetHeader(MessagingConstants.SubjectHeaderName, out subject)
            || TracingConstants.AnonymousSubjectTagValue.Equals(subject, StringComparison.OrdinalIgnoreCase))
        {
            _identityContextSetter.IdentityContext =
                new AnonymousIdentityContext(_staticPermissionsProvider.GetAnonymousPermissions());
        }
        else if (Guid.TryParse(subject, out var userId)
                 && context.TryGetHeader(MessagingConstants.AppHeaderName, out appName))
        {
            var permissions =
                context.TryGetHeader(MessagingConstants.PermissionsHeaderName, out List<string>? permissionNames)
                    ? permissionNames.Select(Permission.Parse).ToList()
                    : [];

            permissions.AddRange(_staticPermissionsProvider.GetUserPermissions());

            _identityContextSetter.IdentityContext = new UserIdentityContext(userId, appName, permissions);
        }
        else
        {
            _identityContextSetter.IdentityContext =
                new SystemIdentityContext(_staticPermissionsProvider.GetSystemPermissions());
        }

        var operationContext = _operationContextAccessor.OperationContext;
        operationContext.AddTag(TracingConstants.SubjectTagName, subject ?? TracingConstants.AnonymousSubjectTagValue);
        operationContext.AddTag(TracingConstants.AppTagName, appName ?? TracingConstants.UnknownAppTagValue);

        await next.Send(context);
    }

    public void Probe(ProbeContext context) => context.CreateFilterScope("IdentityScope");
}