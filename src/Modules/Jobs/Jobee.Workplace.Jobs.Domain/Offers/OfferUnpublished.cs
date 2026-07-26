using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Offers;

public class OfferUnpublished(Guid offerId) : IDomainEvent
{
    public Guid OfferId { get; } = offerId;
}

