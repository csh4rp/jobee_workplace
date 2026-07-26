using Jobee.Workplace.Jobs.Domain.Categories;
using Jobee.Workplace.Shared.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Categories.Repositories;

internal sealed class CategoryRepository : CrudRepository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context)
    {
    }
}