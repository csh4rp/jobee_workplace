namespace Jobee.Workplace.Shared.Domain;

public abstract class Entity<TId> where TId : IEquatable<TId>
{
    private readonly Queue<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected set; } = default!;

    protected void EnqueueEvent(IDomainEvent domainEvent) => _domainEvents.Enqueue(domainEvent);

    public IEnumerable<IDomainEvent> DequeueEvents()
    {
        while (_domainEvents.TryDequeue(out var @event))
        {
            yield return @event;
        }
    }
}