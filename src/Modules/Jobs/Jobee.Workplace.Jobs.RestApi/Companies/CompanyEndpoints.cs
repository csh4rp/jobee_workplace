using Jobee.Workplace.Jobs.Contracts.Companies.Creation;
using Jobee.Workplace.Jobs.Contracts.Companies.Queries;
using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using Jobee.Workplace.Jobs.Contracts.Companies.Updating;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobee.Workplace.Jobs.RestApi.Companies;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/companies")
            .WithTags("Companies");

        group.MapGet("/", GetAllCompanies)
            .Produces<ResultPage<CompanyModel>>();
        group.MapGet("/{companyId:guid}", GetCompany)
            .WithName("GetCompany")
            .Produces<CompanyModel>();
        group.MapPost("/", CreateCompany)
            .Produces<Guid>(StatusCodes.Status201Created);
        group.MapPut("/{companyId:guid}", UpdateCompany)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> GetAllCompanies([FromServices] IMediator mediator,
        [AsParameters] GetCompaniesQuery query,
        CancellationToken cancellationToken)
    {
        var resultPage = await mediator.Send(query, cancellationToken);
        return Results.Ok(resultPage);
    }

    private static async Task<IResult> GetCompany([FromServices] IMediator mediator,
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyQuery();
        query.SetCompanyId(companyId);

        var company = await mediator.Send(query, cancellationToken);
        return Results.Ok(company);
    }

    private static async Task<IResult> CreateCompany([FromServices] IMediator mediator,
        [FromBody] CreateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var companyId = await mediator.Send(command, cancellationToken);
        return Results.Created($"/companies/{companyId}", companyId);
    }

    private static async Task<IResult> UpdateCompany([FromServices] IMediator mediator,
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(companyId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}