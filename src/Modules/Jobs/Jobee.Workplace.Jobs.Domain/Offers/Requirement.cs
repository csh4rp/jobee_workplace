using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Offers;

public class Requirement : Entity<Guid>
{
    public Guid OfferId { get; private set; }
    
    public string Name { get; init; }
    
    public KnowledgeLevel Level { get; init; }

    public Requirement(Guid offerId, string name, KnowledgeLevel level)
    {
        Id = Guid.CreateVersion7();
        OfferId = offerId;
        Name = name;
        Level = level;
    }
}