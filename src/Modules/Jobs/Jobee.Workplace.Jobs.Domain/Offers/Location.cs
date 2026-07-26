using Jobee.Workplace.Jobs.Domain.Shared;
using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Offers;

public class Location : Entity<Guid>
{
    public Guid OfferId { get; init; }
    
    public string Name { get; private set; }
    
    public Coordinates? Coordinates { get; private set; }

    public Address? Address { get; private set; }

    public Location(Guid offerId, string name, Address? address)
    {
        OfferId = offerId;
        Name = name;
        Address = address;
    }

    public void UpdateCoordinates(Coordinates coordinates) => Coordinates = coordinates;
}