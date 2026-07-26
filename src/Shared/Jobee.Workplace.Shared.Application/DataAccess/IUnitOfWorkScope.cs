namespace Jobee.Workplace.Shared.Application.DataAccess;

public interface IUnitOfWorkScope : IAsyncDisposable
{ 
    Task CommitAsync(CancellationToken cancellationToken);
}