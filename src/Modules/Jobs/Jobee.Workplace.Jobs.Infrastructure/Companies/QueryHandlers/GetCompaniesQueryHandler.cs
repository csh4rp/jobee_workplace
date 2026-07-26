using Jobee.Workplace.Jobs.Contracts.Companies.Queries;
using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using Jobee.Workplace.Jobs.Domain.Companies;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Companies.QueryHandlers;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ResultPage<CompanyModel>>
{
    private readonly DbContext _dbContext;

    public GetCompaniesQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultPage<CompanyModel>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<Company>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(c => c.Name.StartsWith(request.Name));
        }

        var items = await query.OrderBy(c => c.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Address = new AddressModel
                {
                    City = c.Address.City,
                    Street = c.Address.Street,
                    PostalCode = c.Address.PostalCode,
                    Country = c.Address.Country,
                    FirstLine = c.Address.FirstLine,
                    SecondLine = c.Address.SecondLine
                }
            })
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        return new ResultPage<CompanyModel>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = count
        };
    }
}

