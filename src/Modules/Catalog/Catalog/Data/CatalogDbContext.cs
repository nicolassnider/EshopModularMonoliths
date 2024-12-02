namespace Catalog.Data;
public class CatalogDbContext : DbContext
{
    // Add-Migration InitialCreate -OutputDir Data/Migrations -Project Catalog -StartupProject Api
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
