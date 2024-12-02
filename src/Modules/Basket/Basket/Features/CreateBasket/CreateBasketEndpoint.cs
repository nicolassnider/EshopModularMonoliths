namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCartDto);
public record CreateBasketResponse(Guid Id);
public class CreateBasketEndpoint :
    ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketHandler request, ISender sender) =>
        {
            var command = request.Adapt<CreateBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateBasketResponse>();
            return Results.Created($"/basket/{response.Id}", response);
        })
            .Produces<CreateBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new basket")
            .WithDescription("Create a new basket");
    }


}
