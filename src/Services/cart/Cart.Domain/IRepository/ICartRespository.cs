using   CartEntity = Cart.Domain.Entities.Cart;
using Shared.Library.Events.Integrations;
namespace Cart.Domain.IRepository;

public interface ICartRespository
{
    Task<CartEntity> GetByUserIdAsync(Guid UserId);
    Task SaveAsync(CartEntity cart);
}
