using Cart.Domain.Entities;

namespace Cart.Application.Features.Cart.Command.AddItem;

public record  AddToCartCommand(Guid UserId, Guid ProductId, int Quantity) :IRequest;
