using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Shared;

public class OfferModel
{
    public required Guid Id { get; init; }

    public required Guid CompanyId { get; init; }

    public required Guid CategoryId { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }

    public required bool IsPublished { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required WorkTypeModel WorkType { get; init; }

    public required ContractTypeModel ContractType { get; init; }

    public required IReadOnlyList<OfferRequirementModel> Requirements { get; init; }

    public required IReadOnlyList<OfferLocationModel> Locations { get; init; }
}