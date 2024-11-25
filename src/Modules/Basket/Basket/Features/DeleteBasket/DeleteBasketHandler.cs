namespace Basket.Basket.Features.DeleteBasket;
public record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("userName is required");
    }
}

internal class DeleteBasketHandler
    (BasketDbContext dbContext)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException(command.UserName);
        }
        dbContext.ShoppingCarts.Remove(shoppingCart);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteBasketResult(true);
    }


}
