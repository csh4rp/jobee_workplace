using System.Text;
using Jobee.Workplace.Shared.RestAPI.Options;
using Microsoft.Extensions.Options;

namespace Jobee.Workplace.Shared.RestAPI.Middlewares;

internal sealed class BasicAuthMiddleware : IMiddleware
{
    private readonly BasicAuthOptions _options;

    public BasicAuthMiddleware(IOptions<BasicAuthOptions> options) => _options = options.Value;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        if (!_options.Paths.Contains(request.Path.Value, StringComparer.InvariantCultureIgnoreCase))
        {
            await next(context);
            return;
        }

        if (request.Headers.Authorization.Count == 0 || request.Headers.Authorization[0] is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var headerValue = request.Headers.Authorization[0]!;
        var parts = headerValue.Split();
        var value = parts[1];

        if (parts.Length != 2)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var buffer = new byte[((value.Length * 3) + 3) / 4];
        if (!Convert.TryFromBase64String(value, buffer, out var bytesWritten))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var usernameAndPassword = Encoding.UTF8.GetString(buffer, 0, bytesWritten).Split(':');
        var username = usernameAndPassword[0];
        var password = usernameAndPassword[1];

        if (username != _options.Username || password != _options.Password)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }
}
