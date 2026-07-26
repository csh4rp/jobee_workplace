using Jobee.Workplace.Jobs.Infrastructure.Categories;
using Jobee.Workplace.Jobs.Infrastructure.Companies;
using Jobee.Workplace.Jobs.Infrastructure.Offers;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Jobs.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCategories();
        services.AddCompanies();
        services.AddOffers();

        return services;
    }
}