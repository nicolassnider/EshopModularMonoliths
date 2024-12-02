namespace Catalog.Products.Features.GetProductById;

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException(request.Id);
        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productDto);
    }
}
