namespace Jobee.Workplace.Jobs.Domain.Categories;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);

    Task UpdateAsync(Category category, CancellationToken cancellationToken);

    Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
