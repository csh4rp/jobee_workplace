using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Queries;

public record GetOfferQuery(Guid OfferId) : IRequest<OfferModel>;