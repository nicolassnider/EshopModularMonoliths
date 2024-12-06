
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
    IBasketRepository repository,
    IBus bus)
    : IRequestHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket =
            await repository
            .GetBasket(
                userName: command.BasketCheckout.UserName,
                asNoTracking: true,
                cancellationToken: cancellationToken);

        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();

        eventMessage.TotalPrice = basket.TotalPrice;

        await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
