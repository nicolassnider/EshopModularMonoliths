namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketRequest(string User);
public record DeleteBasketResponse(bool IsSuccess);

internal class DeleteBasketEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{User}", async (string user, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(user));
            var response = result.Adapt<DeleteBasketRequest>();
            return Results.Ok(response);
        })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete basket")
            .WithDescription("Delete basket");
    }
}
