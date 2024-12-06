namespace Ordering.Orders.Events;
public class BasketCheckoutIntegrationEventHandler(
    ISender sender,
    ILogger<BasketCheckoutIntegrationEventHandler> logger)
    : IConsumer<BasketCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        logger.LogInformation
            ("Integration Event Handler: {IntegrationEvent}", context.Message.GetType().Name);
        var createOrderCommand = MapToCreateOrderCommant(context.Message);
        await sender.Send(createOrderCommand);
    }

    private CreateOrderCommand MapToCreateOrderCommant(BasketCheckoutIntegrationEvent message)
    {
        var addressDto = new AddressDto(
            FirstName: message.FirstName,
            LastName: message.LastName,
            EmailAddress: message.EmailAddress,
            AddressLine: message.AddressLine,
            State: message.State,
            Country: message.Country,
            ZipCode: message.ZipCode
        );
        var paymentDto = new PaymentDto(
            CardName: message.CardName,
            CardNumber: message.CardNumber,
            Expiration: message.Expiration,
            Cvv: message.Cvv,
            PaymentMethod: message.PaymentMethod
            );

        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Items: [
                new OrderItemDto(OrderId: orderId, ProductId: Guid.NewGuid(), Quantity:2,Price:500),
                new OrderItemDto(OrderId: orderId, ProductId: Guid.NewGuid(), Quantity:4,Price:200)
                ]
            );

        return new CreateOrderCommand(orderDto);
    }
}
