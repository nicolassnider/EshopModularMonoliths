using Shared.Data;

namespace Basket;
public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application Use Case Services
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetEntryAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());

        // Data - Infrastructure Services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetServices<ISaveChangesInterceptor>()
                );
            options.UseNpgsql(connectionString);
        }
        );

        services.AddScoped<IDataSeeder, BasketDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        app.UseMigration<BasketDbContext>();
        return app;
    }
}
