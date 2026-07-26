using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Queries;

public record GetOffersQuery : IRequest<ResultPage<OfferModel>>, IPageFilter
{
    public Guid? CompanyId { get; private set; }
    
    public required int PageNumber { get; init; }
    
    public required int PageSize { get; init; }
    
    public void SetCompanyId(Guid companyId) => CompanyId = companyId;
}