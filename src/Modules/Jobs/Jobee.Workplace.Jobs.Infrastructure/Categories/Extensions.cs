using Jobee.Workplace.Jobs.Domain.Categories;
using Jobee.Workplace.Jobs.Infrastructure.Categories.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Jobee.Workplace.Jobs.Infrastructure.Categories;

public static class Extensions
{
    public static IServiceCollection AddCategories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        return services;
    }
}