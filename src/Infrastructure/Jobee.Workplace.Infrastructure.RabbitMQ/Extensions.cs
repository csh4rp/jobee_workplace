using Jobee.Workplace.Shared.Application;
using Jobee.Workplace.Shared.Application.Messaging;
using Jobee.Workplace.Shared.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Jobee.Workplace.Infrastructure.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddMessaging<TDbContext>(this IServiceCollection serviceCollection,
        InfrastructureAssemblyCollection infrastructureAssemblies)
        where TDbContext : DbContext
    {
        bool runConsumers = true;
        
        serviceCollection.AddQuartz()
            .AddQuartzHostedService();

        serviceCollection
            .AddScoped<IMessageBus, MessageBus>()
            .AddMassTransit(c =>
            {
                c.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                c.AddQuartzConsumers();
                c.AddMessageScheduler(MessagingConstants.ScheduleQueueUri);
                c.SetJobConsumerOptions();
                c.AddJobSagaStateMachines().EntityFrameworkRepository(cf =>
                {
                    cf.ExistingDbContext<TDbContext>();
                    cf.UsePostgres();
                });

                if (runConsumers)
                {
                    c.AddConsumers(infrastructureAssemblies.ToArray());
                }

                c.UsingRabbitMq((context, configurator) =>
                {
                    configurator.UseMessageScheduler(MessagingConstants.ScheduleQueueUri);
                    // configurator.UseConsumeFilter(typeof(TracingFilter<>), context);
                    // configurator.UseConsumeFilter(typeof(IdentityFilter<>), context);
                    configurator.ConfigureEndpoints(context);
                });
            });

        return serviceCollection;
    }
}