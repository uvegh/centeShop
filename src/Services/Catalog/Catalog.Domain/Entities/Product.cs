

using Shared.Library.Model;

namespace Catalog.Domain.Entities;

public class Product : BaseEntity
{
    private Product()
    {

    }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    //entry point,assigning values
    public Product(string name, decimal price, int stockQuantitiy)
    {
        Name = name;
        Price = price;
        StockQuantity = stockQuantitiy;

    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));


        if (StockQuantity < quantity)
            throw new InvalidOperationException("Insufficient stock");

        StockQuantity = -quantity;


    }


    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <=0)
        
            throw new ArgumentOutOfRangeException(nameof(newPrice));

        Price = newPrice;

    }

    public static Product Create(string name, decimal price, int stockQuantitiy)
    {
        var newProduct = new Product(name, price, stockQuantitiy);
        return newProduct;

    }
}
