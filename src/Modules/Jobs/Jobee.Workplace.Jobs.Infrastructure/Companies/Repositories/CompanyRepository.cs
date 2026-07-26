using Jobee.Workplace.Jobs.Domain.Companies;
using Jobee.Workplace.Shared.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Companies.Repositories;

internal sealed class CompanyRepository : CrudRepository<Company, Guid>, ICompanyRepository
{
    public CompanyRepository(DbContext context) : base(context)
    {
    }
}

