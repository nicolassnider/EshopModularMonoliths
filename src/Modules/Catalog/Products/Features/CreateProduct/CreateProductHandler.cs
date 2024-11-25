namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(
        ProductDto Product
    )
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
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

internal class CreateProductHandler(
    CatalogDbContext dbContext,
    ILogger<CreateProductHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {


        logger.LogInformation("CreateProductCommandHandler.Handler called with {@Command}", command);

        var product = CreateNewProduct(command.Product);

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        var product = Product.Create(
            id: Guid.NewGuid(),
            name: productDto.Name,
            category: productDto.Category,
            description: productDto.Description,
            imageFile: productDto.ImageFile,
            price: productDto.Price);

        return product;
    }
}
