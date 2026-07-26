using Jobee.Workplace.Jobs.Domain.Offers;
using Jobee.Workplace.Shared.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.Repositories;

internal sealed class OfferRepository : CrudRepository<Offer, Guid>, IOfferRepository
{
    public OfferRepository(DbContext context) : base(context)
    {
    }

    public override async Task<Offer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<Offer>()
                   .Include(o => o.Requirements)
                   .Include(o => o.Locations)
                   .FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken)
               ?? throw new ArgumentException($"Entity of type {nameof(Offer)} with id {id} not found.", nameof(id));
    }
}

