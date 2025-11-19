using Cart.Domain.Entities;

namespace Cart.Application.Features.Command.Cart.AddItem;

public record  AddToCartCommand(Guid UserId, Guid ProductId, int Quantity) :IRequest
    <CartEntity>;
