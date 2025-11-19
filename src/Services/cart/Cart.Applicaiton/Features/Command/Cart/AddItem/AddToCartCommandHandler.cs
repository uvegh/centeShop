using Cart.Application.Interface;
using Cart.Domain.Entities;
using Cart.Domain.IRepository;

namespace Cart.Application.Features.Command.Cart.AddItem;

public class AddToCartCommandHandler : IRequestHandler< AddToCartCommand, CartEntity>
{

    private readonly ICartRespository _cartRepository;
    private readonly ICatalogClient _catalogClient;

    public AddToCartCommandHandler(ICartRespository cartRespository,ICatalogClient catalogClient)
    {
        _cartRepository = cartRespository;
        _catalogClient = catalogClient;


    }


    //public async Task<CartEntity> Handle(AddToCartCommand request, CancellationToken ct)
    //{
    //    Guid userId = request.UserId == Guid.Empty
    //        ? Guid.Parse("00000000-0000-0000-0000-000000000001")
    //        : request.UserId;

    //    var product = await _catalogClient.GetProduct(request.ProductId,ct);

    //    if (product == null) throw new Exception("Product not found");

    //    var cart = await _cartRepository.GetByUserIdAsync(userId);

    //    if (cart != null)
    //    {
    //        cart.AddItem(
    //            request.ProductId,
    //            request.Quantity,
    //            product.Name,
    //            product.Price);

    //        await _cartRepository.SaveCartAsync(userId);
    //        return cart;
    //    }

    //    // new cart
    //    var newCart = new CartEntity(userId);
    //    newCart.AddItem(request.ProductId, request.Quantity, product.Name, product.Price);

    //    await _cartRepository.SaveCartAsync(newCart);
    //    return newCart;
    //}

    public async Task<CartEntity> Handle(AddToCartCommand request, CancellationToken ct)
    {
        // 1. Validate inputs (simplified validation example)
        Guid userId = request.UserId == Guid.Empty
            ? throw new ArgumentException("Invalid User ID", nameof(request.UserId))
            : request.UserId;

        // 2. Fetch required product details to build the CartItem entity
        var product = await _catalogClient.GetProduct(request.ProductId, ct);
        if (product == null)
        {
            throw new Exception("Product not found in catalog.");
        }

        // 3. Create the specific item we intend to add
        var newItem = new CartItem(request.ProductId, request.Quantity, product.Name, product.Price);

        // 4. Use the new List-based repository method to append the item to the Redis List.
        // This is atomic and does not require fetching existing items first.
        await _cartRepository.AddCartItemAsync(userId, newItem);

        // 5. Retrieve the full, updated cart entity to return to the application layer.
        // This method calls ListRangeAsync internally and reconstructs the CartEntity in C# memory.
        var updatedCart = await _cartRepository.GetByUserIdAsync(userId);

        // Ensure the cart was successfully retrieved/created
        if (updatedCart == null)
        {
            throw new ApplicationException("An error occurred while retrieving the updated cart from Redis.");
        }

        return updatedCart;
    }
}

