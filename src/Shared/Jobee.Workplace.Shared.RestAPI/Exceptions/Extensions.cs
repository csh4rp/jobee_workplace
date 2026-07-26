namespace Jobee.Workplace.Shared.RestAPI.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddExceptionHandler<ForbiddenExceptionHandler>()
            .AddExceptionHandler<ValidationExceptionHandler>()
            .AddExceptionHandler<EntityNotFountExceptionHandler>()
            .AddExceptionHandler<ConflictExceptionHandler>()
            .AddExceptionHandler<CatchAllExceptionHandler>();

        return serviceCollection;
    }
}