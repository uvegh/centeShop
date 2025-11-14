
using Ordering.Domain.Common;
using Shared.Library.Model;

namespace Cart.Domain.Entities;


public class Cart:AggregateRoot
{
    

    public Guid UserId { get; private set; }
    //initialize list immediately
    public List<CartItem>? Items { get; set; } = new(); 


    //public Cart(Guid userId, List<CartItem> items)
    //{
    //    UserId = userId;
      

    //}

    private Cart(Guid userId)
    {
        UserId = userId;
    }
    public void   AddItem(Guid productId, int quantity)
    {
        //check if it exist
        var checkItem = Items.FirstOrDefault(x => x.ProductId ==productId);
        if (checkItem != null)
        {
            //if exists increase quantity
            checkItem.Quantity += 1;
        }
        else
        {
            //add new item 
            Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity

            });
        }
       

    }
    public void RemoveItem (CartItem items)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == items.ProductId);
        if (item!=null)
        {
            Items.Remove(item);

        }
        throw new IndexOutOfRangeException();
       
    }

    public static Cart Create(Guid userId)
    {
        var newCart = new Cart (userId);
        return newCart;
    }


    public void Clear() => Items.Clear();
}
