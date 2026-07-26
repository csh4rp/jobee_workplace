using Jobee.Workplace.Jobs.Contracts.Categories.Queries;
using Jobee.Workplace.Jobs.Contracts.Categories.Shared;
using Jobee.Workplace.Jobs.Domain.Categories;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Categories.QueryHandlers;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ResultPage<CategoryModel>>
{
    private readonly DbContext _dbContext;

    public GetCategoriesQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultPage<CategoryModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<Category>().AsQueryable();

        query = request.ParentCategoryId.HasValue 
            ? query.Where(c => c.ParentId == request.ParentCategoryId)
            : query.Where(c => c.ParentId == null);

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }
        
        var items = await query.OrderBy(c => c.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            })
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);

        return new ResultPage<CategoryModel>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = count
        };
    }
}