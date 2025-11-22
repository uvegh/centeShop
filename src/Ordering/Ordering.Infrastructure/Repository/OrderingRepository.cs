


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Interface;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repository;

public class OrderingRepository : IOrderingRepository
{
    private readonly OrderingDbContext _dbContext;
    private readonly ILogger _logger;
    public OrderingRepository(ILogger<OrderingRepository> logger,OrderingDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, CancellationToken ct)
    {
        var newOrder = await _dbContext.AddAsync(order);
        await SaveChangesAsync(ct);

    }

    public async Task<bool> Exists(Guid id, CancellationToken ct)
    {
        var order = await _dbContext.Set<Order>().FirstOrDefaultAsync(x => id == x.Id);
        if (order != null) return true;
        return false;
    }

    public async Task<List<Order>> GetAllAsync( CancellationToken ct)
    {
        var orders = await _dbContext.Set<Order>().ToListAsync();
        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        var order = await _dbContext.Set<Order>().FirstOrDefaultAsync(x => x.Id == id);
        if (order != null)
        {
            return order;
        }
        return null;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
       await  _dbContext.SaveChangesAsync(ct);
    }

   
}
