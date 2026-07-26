namespace Jobee.Workplace.Jobs.Contracts.Categories.Shared;

public record CategoryModel
{
    public required Guid Id { get; init; }
    
    public required Guid? ParentId { get; init; }
    
    public required string Name { get; init; }
}