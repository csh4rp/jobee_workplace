using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Companies.Queries;

public record GetCompanyQuery : IRequest<CompanyModel>
{
    public Guid CompanyId { get; private set; }
    
    public void SetCompanyId(Guid companyId) => CompanyId = companyId;
}