using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Categories;

public class Category : Entity<Guid>
{
    public Guid? ParentId { get; private set; }
    
    public string Name { get; private set; }
    
    public bool IsActive { get; set; }
    
    public Category(Guid? parentId, string name, bool isActive)
    {
        Id = Guid.CreateVersion7();
        ParentId = parentId;
        Name = name;
        IsActive = isActive;
    }

    public void Update(Guid? parentId, string name, bool isActive)
    {
        ParentId = parentId;
        Name = name;
        IsActive = isActive;
    }
}