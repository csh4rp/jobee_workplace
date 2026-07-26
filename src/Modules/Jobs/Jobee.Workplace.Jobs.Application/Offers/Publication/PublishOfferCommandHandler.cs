using Jobee.Workplace.Jobs.Contracts.Offers.Publication;
using Jobee.Workplace.Jobs.Domain.Offers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Offers.Publication;

public class PublishOfferCommandHandler : IRequestHandler<PublishOfferCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<PublishOfferCommandHandler> _logger;

    public PublishOfferCommandHandler(IOfferRepository offerRepository, ILogger<PublishOfferCommandHandler> logger)
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task Handle(PublishOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetByIdAsync(request.OfferId, cancellationToken);

        offer.Publish();

        await _offerRepository.UpdateAsync(offer, cancellationToken);

        _logger.LogInformation("Offer {OfferId} published", offer.Id);
    }
}

