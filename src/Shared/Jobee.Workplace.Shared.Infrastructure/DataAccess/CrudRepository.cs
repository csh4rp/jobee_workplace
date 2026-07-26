using Jobee.Workplace.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Shared.Infrastructure.DataAccess;

public abstract class CrudRepository<TEntity, TId> where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    protected DbContext Context { get; }

    protected CrudRepository(DbContext context)
    {
        Context = context;
    }
    
    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Set<TEntity>().Add(entity);
        return Context.SaveChangesAsync(cancellationToken);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Set<TEntity>().Attach(entity).State = EntityState.Modified;
        }

        return Context.SaveChangesAsync(cancellationToken);
    }
    
    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Set<TEntity>().Remove(entity);
        return Context.SaveChangesAsync(cancellationToken);
    }
    
    public virtual async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken)
            ?? throw new ArgumentException($"Entity of type {typeof(TEntity).Name} with id {id} not found.",  nameof(id));
    }
    
    public virtual async Task<IReadOnlyList<TEntity>> GetAllByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
        var items = await Context.Set<TEntity>()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return items;
    }
}