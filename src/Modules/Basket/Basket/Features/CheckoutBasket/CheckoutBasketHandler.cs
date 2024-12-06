namespace Basket.Basket.Features.CheckoutBasket;
public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : IRequest<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);
public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull()
            .WithMessage("BasketCheckout cannot be null");
        RuleFor(x => x.BasketCheckout.UserName)
            .NotEmpty()
            .WithMessage("UserName cannot be empty");
    }
}
internal class CheckoutBasketHandler(
    BasketDbContext dbContext,
    IBasketRepository repository,
    IBus bus)
    : IRequestHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {

        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var basket = await dbContext.ShoppingCarts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(
                x => x.UserName == command.BasketCheckout.UserName,
                cancellationToken);

            if (basket == null)
            {
                throw new ShoppingCartNotFoundException(command.BasketCheckout.UserName);
            }

            var eventMessage = command.BasketCheckout
                .Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccurredOn = DateTime.UtcNow,
            };

            dbContext.OutboxMessages.Add(outboxMessage);

            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckoutBasketResult(true);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        }

        /* -- Checkout Basket wihouth Outbox
         * var basket =
            await repository
            .GetBasket(
                userName: command.BasketCheckout.UserName,
                asNoTracking: true,
                cancellationToken: cancellationToken);

        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();

        eventMessage.TotalPrice = basket.TotalPrice;

        await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

        return new CheckoutBasketResult(true);*/
    }
}
