using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Shared;

public class OfferLocationModel
{
    public required string Name { get; init; }

    public AddressModel? Address { get; init; }
}

