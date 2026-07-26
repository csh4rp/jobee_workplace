using Jobee.Workplace.Jobs.Domain.Shared;
using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Companies;

public class Company : Entity<Guid>
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Address Address { get; private set; }
    
    public Company(string name, string description, Address address)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        Address = address;
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
}