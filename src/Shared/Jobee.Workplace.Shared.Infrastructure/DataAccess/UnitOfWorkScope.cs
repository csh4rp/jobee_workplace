using Jobee.Workplace.Shared.Application.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace Jobee.Workplace.Shared.Infrastructure.DataAccess;

internal sealed class UnitOfWorkScope : IUnitOfWorkScope, IAsyncDisposable
{
    private readonly IDbContextTransaction _transaction;

    public UnitOfWorkScope(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return _transaction.CommitAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _transaction.DisposeAsync();
    }
}