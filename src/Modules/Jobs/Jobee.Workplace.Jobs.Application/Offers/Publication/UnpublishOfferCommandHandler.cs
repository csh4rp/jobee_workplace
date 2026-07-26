using Jobee.Workplace.Jobs.Contracts.Offers.Publication;
using Jobee.Workplace.Jobs.Domain.Offers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Offers.Publication;

public class UnpublishOfferCommandHandler : IRequestHandler<UnpublishOfferCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<UnpublishOfferCommandHandler> _logger;

    public UnpublishOfferCommandHandler(IOfferRepository offerRepository, ILogger<UnpublishOfferCommandHandler> logger)
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task Handle(UnpublishOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetByIdAsync(request.OfferId, cancellationToken);

        offer.Unpublish();

        await _offerRepository.UpdateAsync(offer, cancellationToken);

        _logger.LogInformation("Offer {OfferId} unpublished", offer.Id);
    }
}

