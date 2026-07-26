using Jobee.Workplace.Jobs.Domain.Companies;
using Jobee.Workplace.Jobs.Infrastructure.Companies.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Jobs.Infrastructure.Companies;

public static class Extensions
{
    public static IServiceCollection AddCompanies(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        
        return services;
    }
}