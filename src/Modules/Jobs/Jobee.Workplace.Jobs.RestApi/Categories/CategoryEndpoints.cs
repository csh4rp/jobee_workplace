using Jobee.Workplace.Jobs.Contracts.Categories.Creation;
using Jobee.Workplace.Jobs.Contracts.Categories.Queries;
using Jobee.Workplace.Jobs.Contracts.Categories.Shared;
using Jobee.Workplace.Jobs.Contracts.Categories.Updating;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobee.Workplace.Jobs.RestApi.Categories;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/categories")
            .WithTags("Categories")
            .WithGroupName("jobs-v1");

        group.MapGet("/", GetCategories)
            .Produces<ResultPage<CategoryModel>>();
        group.MapGet("/{categoryId:guid}", GetCategory)
            .Produces<CategoryModel>();
        group.MapPost("/", CreateCategory)
            .Produces<Guid>(StatusCodes.Status201Created);
        group.MapPut("/{categoryId:guid}", UpdateCategory)
            .Produces<Guid>();
    }

    private static async Task<IResult> GetCategories([FromServices] IMediator mediator,
        [AsParameters] GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(query, cancellationToken);
        return Results.Ok(categories);
    }

    private static async Task<IResult> GetCategory([FromServices] IMediator mediator,
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategoryQuery { CategoryId = categoryId }, cancellationToken);
        return Results.Ok(category);
    }

    private static async Task<IResult> CreateCategory([FromServices] IMediator mediator,
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var categoryId = await mediator.Send(command, cancellationToken);
        return Results.Created($"/categories/{categoryId}", categoryId);
    }

    private static async Task<IResult> UpdateCategory([FromServices] IMediator mediator,
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(categoryId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
