using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetPoductsResult>;

public record GetPoductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetPoductsResult>
{
    public async Task<GetPoductsResult> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken
    )
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);
        var products = await dbContext
            .Products.AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetPoductsResult(
            new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, productDtos)
        );
    }
}
