using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Companies.Queries;

public class GetCompaniesQuery : IRequest<ResultPage<CompanyModel>>, IPageFilter
{
    public required int PageNumber { get; init; }
    
    public required int PageSize { get; init; }
    
    public string? Name { get; init; }
}