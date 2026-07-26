using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Offers.Publication;

public class UnpublishOfferCommand : IRequest
{
    public Guid OfferId { get; private set; }
    
    public void SetOfferId(Guid offerId) =>  OfferId = offerId;

}