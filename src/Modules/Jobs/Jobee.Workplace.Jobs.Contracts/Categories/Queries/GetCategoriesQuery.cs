using Jobee.Workplace.Jobs.Contracts.Categories.Shared;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Categories.Queries;

public class GetCategoriesQuery : IRequest<ResultPage<CategoryModel>>
{
    public required int PageSize { get; set; }
    
    public required int PageNumber { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
    
    public bool? IsActive { get; set; }
}