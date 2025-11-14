

namespace Cart.Domain.Entities;

public  class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public void Increase(int amount) => Quantity += amount;
}
