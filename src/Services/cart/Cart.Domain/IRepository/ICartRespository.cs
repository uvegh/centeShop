using Cart.Domain.Entities;
using Shared.Library.Events.Integrations;
using   CartEntity = Cart.Domain.Entities.Cart;
namespace Cart.Domain.IRepository;

public interface ICartRespository
{
    Task<CartEntity> GetByUserIdAsync(Guid UserId);
    //Task SaveCartAsync(CartEntity cart);
    Task AddCartItemAsync(Guid userId, CartItem item);
}
