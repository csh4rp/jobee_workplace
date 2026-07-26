using Jobee.Workplace.Jobs.Contracts.Offers.Queries;
using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using Jobee.Workplace.Jobs.Domain.Offers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.QueryHandlers;

public class GetOfferQueryHandler : IRequestHandler<GetOfferQuery, OfferModel>
{
    private readonly DbContext _dbContext;

    public GetOfferQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OfferModel> Handle(GetOfferQuery request, CancellationToken cancellationToken)
    {
        var offer = await _dbContext.Set<Offer>()
            .Where(o => o.Id == request.OfferId)
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
            .FirstAsync(cancellationToken);

        return offer;
    }
}


