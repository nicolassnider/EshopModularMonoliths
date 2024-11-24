namespace Catalog.Products.EventHandlers;
public class ProductPriceChangedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: public product price changed integration event for the update basket price
        logger.LogInformation("Domain Event handlerd: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
