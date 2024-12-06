namespace Catalog.Data.Seed;

public static class InitialData
{
    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(
                id: Guid.NewGuid(),
                name: "01Product",
                category: ["Category1", "Category2"],
                imageFile: "/images/01Product.png",
                description: "01ProductDescription",
                price: 100.00m
            ),
            Product.Create(
                id: Guid.NewGuid(),
                name: "02Product",
                category: ["Category3", "Category4"],
                imageFile: "/images/02Product.png",
                description: "02ProductDescription",
                price: 200.00m
            ),
            Product.Create(
                id: Guid.NewGuid(),
                name: "03Product",
                category: ["Category4", "Category5"],
                imageFile: "/images/03Product.png",
                description: "03ProductDescription",
                price: 100.00m
            ),
            Product.Create(
                id: Guid.NewGuid(),
                name: "04Product",
                category: ["Category1", "Category6"],
                imageFile: "/images/04Product.png",
                description: "04ProductDescription",
                price: 100.00m
            )
        };
}
