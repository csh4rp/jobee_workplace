using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Offers;

public class Offer : Entity<Guid>
{
    public Guid CompanyId { get; private set; }

    public Guid CategoryId { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    
    public bool IsPublished { get; private set; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }

    public WorkType WorkType { get; private set; }
    
    public ContractType ContractType { get; private set; }
    
    public IReadOnlyList<Requirement> Requirements { get; private set; }
    
    public IReadOnlyList<Location> Locations { get; private set; }

    public Offer(Guid id, Guid companyId, Guid categoryId, DateTimeOffset createdAt,
        string title, string description, WorkType workType, ContractType contractType,
        IEnumerable<Requirement> requirements,
        IEnumerable<Location> locations)
    {
        Id = id;
        CompanyId = companyId;
        CategoryId = categoryId;
        CreatedAt = createdAt;
        Title = title;
        Description = description;
        WorkType = workType;
        ContractType = contractType;
        Requirements = requirements as IReadOnlyList<Requirement> ?? requirements.ToList();
        Locations = locations as IReadOnlyList<Location> ?? locations.ToList();
    }

    public void Update(Guid categoryId, string title, string description,
        WorkType workType, ContractType contractType,
        IEnumerable<Requirement> requirements,
        IEnumerable<Location> locations)
    {
        CategoryId = categoryId;
        Title = title;
        Description = description;
        WorkType = workType;
        ContractType = contractType;
        Requirements = requirements as IReadOnlyList<Requirement> ?? requirements.ToList();
        Locations = locations as IReadOnlyList<Location> ?? locations.ToList();
        
        EnqueueEvent(new OfferUpdated(Id));
    }

    public void Publish()
    {
        if (IsPublished)
        {
            throw new InvalidOperationException("Cannot publish offers twice");
        }
        
        IsPublished = true;
        EnqueueEvent(new OfferPublished(Id));
    }
    
    public void Unpublish()
    {
        if (!IsPublished)
        {
            throw new InvalidOperationException("Cannot unpublish an offer that is not published");
        }

        IsPublished = false;
        EnqueueEvent(new OfferUnpublished(Id));
    }
}