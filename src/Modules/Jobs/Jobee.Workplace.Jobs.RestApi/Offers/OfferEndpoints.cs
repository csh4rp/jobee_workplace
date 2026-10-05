using Jobee.Workplace.Jobs.Contracts.Offers.Creation;
using Jobee.Workplace.Jobs.Contracts.Offers.Publication;
using Jobee.Workplace.Jobs.Contracts.Offers.Queries;
using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Offers.Updating;
using Jobee.Workplace.Shared.Contracts.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobee.Workplace.Jobs.RestApi.Offers;

public static class OfferEndpoints
{
    public static void MapOfferEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/offers")
            .WithTags("Offers");

        group.MapGet("/", GetOffers)
            .Produces<ResultPage<OfferModel>>();
        group.MapGet("/{offerId:guid}", GetOffer)
            .Produces<OfferModel>();
        group.MapPut("/{offerId:guid}", UpdateOffer)
            .Produces(StatusCodes.Status204NoContent);
        group.MapPost("/{offerId:guid}/publish", PublishOffer)
            .Produces(StatusCodes.Status204NoContent);
        group.MapPost("/{offerId:guid}/unpublish", UnpublishOffer)
            .Produces(StatusCodes.Status204NoContent);

        app.MapPost("/companies/{companyId:guid}/offers", CreateOffer)
            .WithTags("Offers")
            .Produces<Guid>(StatusCodes.Status201Created);
    }

    private static async Task<IResult> GetOffers([FromServices] IMediator mediator,
        [AsParameters] GetOffersQuery query,
        [FromQuery] Guid? companyId,
        CancellationToken cancellationToken)
    {
        if (companyId.HasValue)
        {
            query.SetCompanyId(companyId.Value);
        }

        var offers = await mediator.Send(query, cancellationToken);
        return Results.Ok(offers);
    }

    private static async Task<IResult> GetOffer([FromServices] IMediator mediator,
        [FromRoute] Guid offerId,
        CancellationToken cancellationToken)
    {
        var offer = await mediator.Send(new GetOfferQuery(offerId), cancellationToken);
        return Results.Ok(offer);
    }

    private static async Task<IResult> CreateOffer([FromServices] IMediator mediator,
        [FromRoute] Guid companyId,
        [FromBody] CreateOfferCommand command,
        CancellationToken cancellationToken)
    {
        command.SetCompanyId(companyId);
        var offerId = await mediator.Send(command, cancellationToken);
        return Results.Created($"/offers/{offerId}", offerId);
    }

    private static async Task<IResult> UpdateOffer([FromServices] IMediator mediator,
        [FromRoute] Guid offerId,
        [FromBody] UpdateOfferCommand command,
        CancellationToken cancellationToken)
    {
        command.SetOfferId(offerId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> PublishOffer([FromServices] IMediator mediator,
        [FromRoute] Guid offerId,
        CancellationToken cancellationToken)
    {
        var command = new PublishOfferCommand();
        command.SetOfferId(offerId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> UnpublishOffer([FromServices] IMediator mediator,
        [FromRoute] Guid offerId,
        CancellationToken cancellationToken)
    {
        var command = new UnpublishOfferCommand();
        command.SetOfferId(offerId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
