using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Offers;

public record OfferUpdated(Guid OfferId) : IDomainEvent;
