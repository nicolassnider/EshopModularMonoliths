namespace Catalog.Products.Features.GetProducts;
public record GetProductsRequest();
public record GetPoductsResponse(IEnumerable<ProductDto> Products);
public class GetProductsEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = result.Adapt<GetPoductsResponse>();
            return Results.Ok(response);
        })
            .WithName("GetProducts")
            .Produces<GetPoductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get products")
            .WithDescription("Get products");
    }
}
