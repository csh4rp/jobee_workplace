using Jobee.Workplace.Jobs.Contracts.Offers.Updating;
using Jobee.Workplace.Jobs.Domain.Offers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Offers.Updating;

public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<UpdateOfferCommandHandler> _logger;

    public UpdateOfferCommandHandler(IOfferRepository offerRepository, ILogger<UpdateOfferCommandHandler> logger)
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetByIdAsync(request.OfferId, cancellationToken);

        var requirements = request.Requirements.Select(r => r.ToRequirement(offer.Id));

        var locations = request.Locations.Select(l => l.ToLocation(offer.Id));

        offer.Update(
            request.CategoryId,
            request.Title,
            request.Description, 
            request.WorkType.ToWorkType(),
            request.ContractType.ToContractType(),
            requirements,
            locations);

        await _offerRepository.UpdateAsync(offer, cancellationToken);

        _logger.LogInformation("Offer {OfferId} updated", offer.Id);
    }
}

