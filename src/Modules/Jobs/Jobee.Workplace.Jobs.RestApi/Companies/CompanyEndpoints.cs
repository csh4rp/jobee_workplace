using Jobee.Workplace.Jobs.Contracts.Companies.Queries;
using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobee.Workplace.Jobs.RestApi.Companies;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/companies")
            .WithTags("Companies");

        group.MapGet("/", GetAllCompanies)
            .WithName("GetAllCompanies");
    }
    
    private static async Task<IResult> GetAllCompanies([FromServices] IMediator mediator,
        [AsParameters] GetCompaniesQuery query,
        CancellationToken cancellationToken)
    {
        var companies = await mediator.Send(query, cancellationToken);
        return Results.Ok(companies);
    }
}