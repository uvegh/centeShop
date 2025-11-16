

using Catalog.Domain.Entities;
using Shared.Library.Interface;

namespace Catalog.Domain.Interfaces;



public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id);
    Task SaveChangesAsync(CancellationToken ct);
    
}
