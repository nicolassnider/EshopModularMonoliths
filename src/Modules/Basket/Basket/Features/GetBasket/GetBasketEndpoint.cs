namespace Basket.Basket.Features.GetBasket;
public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCartDto ShoppingCart);
public class GetBasketEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{UserName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        }).WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get product by Id")
            .WithDescription("Get product by Id")
            .RequireAuthorization();

    }
}
