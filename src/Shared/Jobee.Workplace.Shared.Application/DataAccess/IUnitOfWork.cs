namespace Jobee.Workplace.Shared.Application.DataAccess;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> BeginScopeAsync(CancellationToken cancellationToken);
}