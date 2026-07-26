using Jobee.Workplace.Jobs.Contracts.Offers.Queries;
using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using Jobee.Workplace.Jobs.Domain.Offers;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.QueryHandlers;

public class GetOffersQueryHandler : IRequestHandler<GetOffersQuery, ResultPage<OfferModel>>
{
    private readonly DbContext _dbContext;

    public GetOffersQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultPage<OfferModel>> Handle(GetOffersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<Offer>().AsQueryable();

        if (request.CompanyId.HasValue)
        {
            query = query.Where(o => o.CompanyId == request.CompanyId.Value);
        }

        var items = await query.OrderByDescending(o => o.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new OfferModel
            {
                Id = o.Id,
                CompanyId = o.CompanyId,
                CategoryId = o.CategoryId,
                CreatedAt = o.CreatedAt,
                IsPublished = o.IsPublished,
                Title = o.Title,
                Description = o.Description,
                WorkType = (WorkTypeModel)o.WorkType,
                ContractType = (ContractTypeModel)o.ContractType,
                Requirements = o.Requirements.Select(r => new OfferRequirementModel
                {
                    Name = r.Name,
                    Level = (KnowledgeLevelModel)r.Level
                }).ToList(),
                Locations = o.Locations.Select(l => new OfferLocationModel
                {
                    Name = l.Name,
                    Address = l.Address != null ? new AddressModel
                    {
                        City = l.Address.City,
                        Street = l.Address.Street,
                        PostalCode = l.Address.PostalCode,
                        Country = l.Address.Country,
                        FirstLine = l.Address.FirstLine,
                        SecondLine = l.Address.SecondLine
                    } : null
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        return new ResultPage<OfferModel>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = count
        };
    }
}


