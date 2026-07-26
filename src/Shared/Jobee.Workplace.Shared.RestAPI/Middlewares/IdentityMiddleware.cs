using System.Security.Claims;
using Jobee.Workplace.Shared.Application.Identity;
using Jobee.Workplace.Shared.Application.Tracing;

namespace Jobee.Workplace.Shared.RestAPI.Middlewares;

internal sealed class IdentityMiddleware : IMiddleware
{
    private readonly IConfiguration _configuration;
    private readonly IIdentityContextSetter _identityContextSetter;
    private readonly ILogger<IdentityMiddleware> _logger;
    private readonly IOperationContextAccessor _operationContextAccessor;
    private readonly StaticPermissionsProvider _permissionsProvider;

    public IdentityMiddleware(IIdentityContextSetter identityContextSetter,
        IOperationContextAccessor operationContextAccessor,
        IConfiguration configuration,
        StaticPermissionsProvider permissionsProvider,
        ILogger<IdentityMiddleware> logger)
    {
        _identityContextSetter = identityContextSetter;
        _operationContextAccessor = operationContextAccessor;
        _configuration = configuration;
        _permissionsProvider = permissionsProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var operationContext = _operationContextAccessor.OperationContext;

        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("User not authenticated, proceeding as anonymous");
            operationContext.AddTag(TracingConstants.SubjectTagName, TracingConstants.AnonymousSubjectTagValue);
            operationContext.AddTag(TracingConstants.AppTagName, TracingConstants.UnknownAppTagValue);

            _identityContextSetter.IdentityContext =
                new AnonymousIdentityContext(_permissionsProvider.GetAnonymousPermissions());

            await next(context);
            return;
        }

        var permissionClaimName = _configuration.GetSection("Identity:PermissionClaimName")
            .Get<string>() ?? ClaimTypes.Role;

        var subject = context.User.FindFirstValue("sub")
                      ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var app = context.User.FindFirstValue("azc")
                  ?? context.User.FindFirstValue("azp");

        if (!Guid.TryParse(subject, out var userId))
        {
            _logger.LogError("Could not parse subject: '{Subject}' as GUID, returning Unauthorized", subject);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (string.IsNullOrEmpty(app))
        {
            _logger.LogError("Could not find 'azc' or 'azp' claim, returning Unauthorized");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var permissions = context.User.FindAll(permissionClaimName)
            .Select(r => Permission.Parse(r.Value))
            .ToList();

        permissions.AddRange(_permissionsProvider.GetUserPermissions());

        _logger.LogInformation("Authenticated as user: '{UserId}' for app: '{App}'",
            subject, app);

        _identityContextSetter.IdentityContext = new UserIdentityContext(userId, app, permissions);
        operationContext.AddTag(TracingConstants.SubjectTagName, subject);
        operationContext.AddTag(TracingConstants.AppTagName, app);

        await next(context);
    }
}
