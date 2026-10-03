using Jobee.Workplace.Jobs.Contracts.Categories.Creation;
using Jobee.Workplace.Jobs.Domain.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Categories.Creation;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(
            request.ParentId,
            request.Name,
            request.IsActive);

        await _categoryRepository.AddAsync(category, cancellationToken);

        _logger.LogInformation("Category {CategoryId} created", category.Id);
        
        return category.Id;
    }
}

