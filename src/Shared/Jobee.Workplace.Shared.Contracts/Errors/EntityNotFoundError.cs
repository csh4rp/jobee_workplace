namespace Jobee.Workplace.Shared.Contracts.Errors;

public class EntityNotFoundError : Error
{
    public EntityNotFoundError(string entityName, object entityId)
        : base(ErrorCodes.EntityNotFound, $"Entity: '{entityName}' with ID: '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId.ToString()!;
    }

    public string EntityName { get; init; }

    public string EntityId { get; init; }

}