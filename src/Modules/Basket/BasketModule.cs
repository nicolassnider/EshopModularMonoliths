namespace Basket;
public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application Use Case Services


        services.AddScoped<IBasketRepository, BasketRepository>();

        /*
         * The key insight here is that we're using the service provider to resolve dependencies 
         * for the CachedBasketRepository instance. By doing so, we can decouple the CachedBasketRepository 
         * from the specific implementation of BasketRepository and IDistributedCache.
         */
        /* services.AddScoped<IBasketRepository>(provider =>
        {
            var basketRepository = provider.GetRequiredService<BasketRepository>();
            return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
        });*/

        services.Decorate<IBasketRepository, CachedBasketRepository>();

        // 3. Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        app.UseMigration<BasketDbContext>();
        return app;
    }
}
