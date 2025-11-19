

using Cart.Domain.Entities;

namespace Cart.Application.DTOs;

public record CartDto(Guid UserId,List<CartItem>? Items);

