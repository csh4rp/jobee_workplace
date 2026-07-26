namespace Jobee.Workplace.Shared.Domain;

public interface IEntity
{
    IEnumerable<IDomainEvent> DequeueEvents();
}