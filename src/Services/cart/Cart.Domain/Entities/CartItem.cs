

namespace Cart.Domain.Entities;

public  class CartItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string ProductName { get; private set; }

    public decimal UnitPrice { get; private set; }
   

    public CartItem(Guid productId, int quantity, string productName, decimal unitPrice )
    {
        ProductId = productId;
        Quantity = quantity;
        ProductName = productName;
        UnitPrice = unitPrice;

    }

    public void Increase(int amount) => Quantity += amount;
}
