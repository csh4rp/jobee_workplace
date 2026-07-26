namespace Jobee.Workplace.Jobs.Domain.Companies;

public interface ICompanyRepository
{
    Task AddAsync(Company company, CancellationToken cancellationToken);

    Task UpdateAsync(Company company, CancellationToken cancellationToken);

    Task<Company> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
