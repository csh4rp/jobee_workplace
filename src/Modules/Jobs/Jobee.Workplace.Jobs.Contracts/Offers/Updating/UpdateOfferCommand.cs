using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Updating;

public class UpdateOfferCommand : IRequest
{
    public Guid OfferId { get; private set; }

    public Guid CompanyId { get; private set; }

    public required Guid CategoryId { get; init; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required WorkTypeModel WorkType { get; init; }

    public required ContractTypeModel ContractType { get; init; }

    public required IReadOnlyCollection<OfferRequirementModel> Requirements { get; init; }

    public required IReadOnlyCollection<OfferLocationModel> Locations { get; init; }

    public void SetOfferId(Guid id) => OfferId = id;
    
    public void SetCompanyId(Guid id) => CompanyId = id;
}

