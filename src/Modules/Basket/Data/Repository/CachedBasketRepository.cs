﻿namespace Basket.Data.Repository;
public class CachedBasketRepository
    (
    IBasketRepository repository,
    IDistributedCache cache)
    : IBasketRepository
{

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };

    public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return await repository.GetBasket(
            userName: userName,
            asNoTracking: asNoTracking,
            cancellationToken: cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            // Deserialize            
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
        }

        var basket = await repository.GetBasket(userName, asNoTracking, cancellationToken);

        // Serialize
        await cache.SetStringAsync(
            userName,
            JsonSerializer.Serialize(basket, _options),
            cancellationToken
            );

        return basket;

    }

    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {

        await repository.CreateBasket(
            basket: basket,
            cancellationToken: cancellationToken);

        await cache.SetStringAsync(
            basket.UserName,
            JsonSerializer.Serialize(basket, _options),
            cancellationToken
            );

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(
            userName: userName,
            cancellationToken: cancellationToken);

        await cache.RemoveAsync(
            userName,
            cancellationToken
            );

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result = await repository.SaveChangesAsync(
            userName: userName,
            cancellationToken: cancellationToken);

        // clear cache
        if (userName is not null)
        {
            await cache.RemoveAsync(
                userName,
                cancellationToken);
        }

        return result;
    }
}
