using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Shared.Application.Tracing;

public static class Extensions
{
    public static IServiceCollection AddTracingServices(this IServiceCollection services)
    {
        return services.AddScoped<IOperationContextAccessor>(sp => sp.GetRequiredService<OperationContextWrapper>())
            .AddScoped<IOperationContextSetter>(sp => sp.GetRequiredService<OperationContextWrapper>())
            .AddScoped<OperationContextWrapper>();
    }
}