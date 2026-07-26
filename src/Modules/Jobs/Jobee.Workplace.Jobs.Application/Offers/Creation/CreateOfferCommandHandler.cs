using Jobee.Workplace.Jobs.Contracts.Offers.Creation;
using Jobee.Workplace.Jobs.Domain.Offers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Offers.Creation;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<CreateOfferCommandHandler> _logger;

    public CreateOfferCommandHandler(IOfferRepository offerRepository, ILogger<CreateOfferCommandHandler> logger)
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var offerId = Guid.CreateVersion7();

        var requirements = request.Requirements.Select(r => r.ToRequirement(offerId));
        var locations = request.Locations.Select(l => l.ToLocation(offerId));

        var offer = new Offer(
            offerId,
            request.CompanyId,
            request.CategoryId,
            DateTimeOffset.UtcNow,
            request.Title,
            request.Description,
            request.WorkType.ToWorkType(),
            request.ContractType.ToContractType(),
            requirements,
            locations);

        await _offerRepository.AddAsync(offer, cancellationToken);

        _logger.LogInformation("Offer {OfferId} created", offer.Id);

        return offer.Id;
    }
}


