namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(
        ProductDto Product
    )
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool Result);

internal class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync([command.Product.Id], cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product {command.Product.Id} not found");
        }

        UpdateProductWithNewValues(product, command.Product);

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private void UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            name: productDto.Name,
            category: productDto.Category,
            description: productDto.Description,
            imageFile: productDto.ImageFile,
            price: productDto.Price
            );
    }
}
