namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator
    : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(
        UpdateItemPriceInBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        // find shopping cart items with a give productId
        var itemsToUpdate = await dbContext
            .ShoppingCartItems.Where(x => x.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        if (!itemsToUpdate.Any())
        {
            return new UpdateItemPriceInBasketResult(false);
        }

        // iteate items and update price with incoming command.price

        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.Price);
        }

        //save to database

        await dbContext.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateItemPriceInBasketResult(true);
    }
}
