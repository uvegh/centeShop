

using Ordering.Domain.Entities;

namespace Ordering.Domain.Interface;

public  interface IOrderingRepository
{

    Task<Order?> GetOrderByIdAsync(Guid id);
    Task <List<Order>> GetAllAsync  (CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<bool> Exists(Guid id, CancellationToken ct);


}
