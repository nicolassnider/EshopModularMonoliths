namespace Ordering.Orders.Features.GetOrderById;

public record GetOrderByIdResponse(OrderDto Order);

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                pattern: "/orders/{id}",
                handler: async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetOrderByIdQuery(id));

                    var response = result.Adapt<GetOrderByIdResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("GetOrderById")
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Order By Id")
            .WithDescription("Get Order By Id");
    }
}
