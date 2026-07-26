using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Shared;

public class OfferRequirementModel
{
    public required string Name { get; init; }

    public required KnowledgeLevelModel Level { get; init; }
}

