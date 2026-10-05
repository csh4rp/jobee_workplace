using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Categories.Updating;

public class UpdateCategoryCommand : IRequest
{
    public Guid Id { get; private set; }
    
    public Guid? ParentId { get; init; }
    
    public bool IsActive { get; init; }
    
    public required string Name { get; init; }
    
    public void SetId(Guid id) => Id = id;
}

