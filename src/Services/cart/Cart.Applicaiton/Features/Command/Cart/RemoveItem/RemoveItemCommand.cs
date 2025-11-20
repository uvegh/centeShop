

namespace Cart.Application.Features.Command.Cart.RemoveItem
{
    public record  RemoveItemCommand(Guid UserId, Guid ProductId) :IRequest<bool>;
    
   
}
