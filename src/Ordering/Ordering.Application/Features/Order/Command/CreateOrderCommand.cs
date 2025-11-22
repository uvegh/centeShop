
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Order.Command;

public record CreateOrderCommand( Guid
     UserId, List<OrderItem> Items, decimal TotalAmount,string Status ):IRequest<Guid>;

