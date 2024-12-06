namespace Basket.Basket.Exceptions;

public class ShoppingCartNotFoundException : NotFoundException
{
    public ShoppingCartNotFoundException(string userName)
        : base("Shopping Cart", userName) { }
}
