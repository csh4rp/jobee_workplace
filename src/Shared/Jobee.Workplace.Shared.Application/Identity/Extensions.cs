using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Shared.Application.Identity;

public static class Extensions
{
    public static IServiceCollection AddIdentityContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IIdentityContextSetter>(sp => sp.GetRequiredService<IdentityContextWrapper>())
            .AddScoped<IIdentityContextAccessor>(sp => sp.GetRequiredService<IdentityContextWrapper>())
            .AddScoped<IdentityContextWrapper>()
            .AddSingleton<StaticPermissionsProvider>();

        return serviceCollection;
    }
}