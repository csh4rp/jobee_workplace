using Jobee.Workplace.Jobs.Contracts.Categories.Updating;
using Jobee.Workplace.Jobs.Domain.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Categories.Updating;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        category.Update(
            request.ParentId,
            request.Name,
            request.IsActive);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        _logger.LogInformation("Category {CategoryId} updated", category.Id);

        return category.Id;
    }
}

