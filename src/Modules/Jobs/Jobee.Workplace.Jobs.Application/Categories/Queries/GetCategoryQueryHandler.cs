using Jobee.Workplace.Jobs.Contracts.Categories.Queries;
using Jobee.Workplace.Jobs.Contracts.Categories.Shared;
using Jobee.Workplace.Jobs.Domain.Categories;
using MediatR;

namespace Jobee.Workplace.Jobs.Application.Categories.Queries;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModel> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        
        return new CategoryModel
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name
        };
    }
}

