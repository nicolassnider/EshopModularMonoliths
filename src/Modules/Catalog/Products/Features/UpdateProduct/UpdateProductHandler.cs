namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(
        ProductDto Product
    )
    : ICommand<UpdateProductResult>;

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
        RuleFor(x => x.Product.Category)
            .NotEmpty()
            .WithMessage("Category is required.");
        RuleFor(x => x.Product.Description)
            .NotEmpty()
            .WithMessage("Description is required.");
        RuleFor(x => x.Product.ImageFile)
            .NotEmpty()
            .WithMessage("ImageFile is required.");
        RuleFor(x => x.Product.Price)
            .GreaterThanOrEqualTo(0)
            .NotEmpty()
            .WithMessage("Price is required.");
    }
}

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync([command.Product.Id], cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Product.Id);
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
