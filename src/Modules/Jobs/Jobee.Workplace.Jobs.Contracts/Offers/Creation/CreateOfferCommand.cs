using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Creation;

public class CreateOfferCommand : IRequest<Guid>
{
    public Guid CompanyId { get; private set; }

    public required Guid CategoryId { get; init; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required WorkTypeModel WorkType { get; init; }

    public required ContractTypeModel ContractType { get; init; }

    public required IReadOnlyCollection<OfferRequirementModel> Requirements { get; init; }

    public required IReadOnlyCollection<OfferLocationModel> Locations { get; init; }
    
    public void SetCompanyId(Guid companyId) => CompanyId = companyId;
}

