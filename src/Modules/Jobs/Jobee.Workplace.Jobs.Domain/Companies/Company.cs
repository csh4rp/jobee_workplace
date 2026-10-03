using Jobee.Workplace.Jobs.Domain.Shared;
using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Companies;

public class Company : Entity<Guid>
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Address Address { get; private set; }

    private Company(Guid id, string name, string description, Address address)
    {
        Id = id;
        Name = name;
        Description = description;
        Address = address;
    }

    public Company(string name, string description, Address address) : this(Guid.CreateVersion7(), name, description, address)
    {
        EnqueueEvent(new CompanyCreated(Id));
    }

    public void Update(string name, string description, Address address)
    {
        if (Name == name && Description == description && Address == address)
        {
            return;
        }
        
        Name = name;
        Description = description;
        Address = address;
        EnqueueEvent(new CompanyUpdated(Id));
    }
    
    public static Company Create(Guid id, string name, string description, Address address) => 
        new(id, name, description, address);
}