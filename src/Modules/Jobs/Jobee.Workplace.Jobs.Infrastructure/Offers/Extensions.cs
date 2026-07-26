using Jobee.Workplace.Jobs.Domain.Offers;
using Jobee.Workplace.Jobs.Infrastructure.Offers.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers;

public static class Extensions
{
    public static IServiceCollection AddOffers(this IServiceCollection services)
    {
        services.AddScoped<IOfferRepository, OfferRepository>();

        return services;
    }
}