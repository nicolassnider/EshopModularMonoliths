namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name is required");
    }
}

internal class CreateOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken
    )
    {
        var order = CreateNewOrder(command.Order);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(
            firstName: orderDto.ShippingAddress.FirstName,
            lastName: orderDto.ShippingAddress.LastName,
            emailAddress: orderDto.ShippingAddress.EmailAddress,
            addressLine: orderDto.ShippingAddress.AddressLine,
            country: orderDto.ShippingAddress.Country,
            state: orderDto.ShippingAddress.State,
            zipCode: orderDto.ShippingAddress.ZipCode
        );

        var billingAddress = Address.Of(
            firstName: orderDto.BillingAddress.FirstName,
            lastName: orderDto.BillingAddress.LastName,
            emailAddress: orderDto.BillingAddress.EmailAddress,
            addressLine: orderDto.BillingAddress.AddressLine,
            country: orderDto.BillingAddress.Country,
            state: orderDto.BillingAddress.State,
            zipCode: orderDto.BillingAddress.ZipCode
        );
        var newOrder = Order.Create(
            id: Guid.NewGuid(),
            customerId: orderDto.CustomerId,
            orderName: $"{orderDto.OrderName}_{new Random().Next()}",
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(
                cardName: orderDto.Payment.CardName,
                cardNumber: orderDto.Payment.CardNumber,
                expiration: orderDto.Payment.Expiration,
                cvv: orderDto.Payment.Cvv,
                paymentMethod: orderDto.Payment.PaymentMethod
            )
        );

        orderDto.Items.ForEach(item =>
        {
            newOrder.Add(item.ProductId, item.Quantity, item.Price);
        });

        return newOrder;
    }
}
