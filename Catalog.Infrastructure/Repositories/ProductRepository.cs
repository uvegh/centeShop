using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Model.Pagination;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _dbContext;
    
    public ProductRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;


    }

   public  async Task AddAsync(Product product,CancellationToken ct)
    {
        
        await _dbContext.Set<Product>().AddAsync(product);
        await SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var product = await _dbContext.Set<Product
            >().FirstOrDefaultAsync(x=>x.Id==id);
      
        return true && product != null;
  
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken ct)
    {
        var products = await _dbContext.Set<Product>().ToListAsync();
        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

  public async  Task SaveChangesAsync(CancellationToken ct)
    {
        
        await _dbContext.SaveChangesAsync(ct);
    }

  
}
