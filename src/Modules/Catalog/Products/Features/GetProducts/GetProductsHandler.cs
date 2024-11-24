namespace Catalog.Products.Features.GetProducts;
public record GetProductsQuery()
    : IQuery<GetPoductsResult>;
public record GetPoductsResult(IEnumerable<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetPoductsResult>
{
    public async Task<GetPoductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetPoductsResult(productDtos);
    }

}
