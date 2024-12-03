using Microsoft.Extensions.Logging;

namespace Basket.Basket.EventHandlers;
public class ProductPriceChangedIntegrationEventHandler
    (ILogger<ProductPriceChangedIntegrationEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event Handled: {IntegrationEvent}", context.Message.GetType().Name);
        // find basket item 

        // mediatr new command and handler to find and update prices
        return Task.CompletedTask;
    }
}
