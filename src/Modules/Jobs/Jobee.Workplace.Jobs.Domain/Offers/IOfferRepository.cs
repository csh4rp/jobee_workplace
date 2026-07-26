namespace Jobee.Workplace.Jobs.Domain.Offers;

public interface IOfferRepository
{
    Task AddAsync(Offer offer, CancellationToken cancellationToken);

    Task UpdateAsync(Offer offer, CancellationToken cancellationToken);

    Task<Offer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
