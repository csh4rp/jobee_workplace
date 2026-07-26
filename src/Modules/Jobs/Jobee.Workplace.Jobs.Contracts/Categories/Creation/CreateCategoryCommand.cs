using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Categories.Creation;

public class CreateCategoryCommand : IRequest<Guid>
{
    public Guid? ParentId { get; init; }
    
    public bool IsActive { get; init; }
    
    public required string Name { get; init; }
}