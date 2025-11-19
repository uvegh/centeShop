



namespace Cart.Application.Features.Query.Cart;

public record  GetUserCartQuery(Guid UserId):IRequest<CartDto>;
