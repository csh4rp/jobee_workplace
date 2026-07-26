using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Shared.Application.Pipeline;

public static class Extensions
{
    public static IServiceCollection AddRequestPipeline(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(c =>
        {
            c.AddOpenBehavior(typeof(RequestTracingDecorator<,>));
            c.AddOpenBehavior(typeof(RequestTransactionalDecorator<,>));
            c.RegisterServicesFromAssemblies(assemblies);
        });

        services.TryDecorate(typeof(INotificationHandler<>), typeof(EventTracingDecorator<>));
        services.TryDecorate(typeof(INotificationHandler<>), typeof(EventTransactionalDecorator<>));

        return services;
    }
}