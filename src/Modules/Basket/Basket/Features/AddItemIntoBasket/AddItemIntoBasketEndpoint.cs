namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(string userName, ShoppingCartItemDto shoppingCartItem);

public record AddItemIntoBaskeResponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    // /basket/userName/items
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/basket/{userName}/items",
                async (
                    [FromRoute] string userName,
                    [FromBody] AddItemIntoBasketRequest request,
                    ISender sender
                ) =>
                {
                    var command = new AddItemIntoBasketCommand(userName, request.shoppingCartItem);

                    var result = await sender.Send(command);
                    var response = result.Adapt<AddItemIntoBaskeResponse>();
                    return Results.Created($"/basket/{response.Id}", response);
                }
            )
            .WithName("AddItemIntoBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Add Item into Basket")
            .WithDescription("Add Item into Basket")
            .RequireAuthorization();
    }
}
