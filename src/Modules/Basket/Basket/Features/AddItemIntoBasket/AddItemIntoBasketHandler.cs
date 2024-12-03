namespace Basket.Basket.Features.AddItemIntoBasket;
public record AddItemIntoBasketCommand(
    string UserName,
    ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;
public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator
    : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName cannot be empty");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId cannot be empty");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity cannot be empty");
    }
}
internal class AddItemIntoBasketHandler
    (
    IBasketRepository repository,
    ISender sender)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {

        var shoppingCart = await repository.GetBasket(
            command.UserName,
            asNoTracking: false,
            cancellationToken: cancellationToken);

        var result = await sender.Send(
            new GetProductByIdQuery(command.ShoppingCartItem.ProductId), cancellationToken
            );

        shoppingCart.AddItem(
            productId: command.ShoppingCartItem.ProductId,
            quantity: command.ShoppingCartItem.Quantity,
            color: command.ShoppingCartItem.Color,
            price: result.Product.Price,
            productName: result.Product.Name);

        await repository.SaveChangesAsync(
            userName: command.UserName,
            cancellationToken: cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
