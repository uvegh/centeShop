using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data;

public class CatalogDbContext:DbContext
{

    public CatalogDbContext( DbContextOptions<CatalogDbContext>options) : base(options) { }


    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //get enetity product builder on modelcreating
        modelBuilder.Entity<Product>((entity) =>
        {
            //name of table
            entity.ToTable("Products");
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

            entity.HasData(
                new Product("Lenovo", 999.99m, 10)
                {
                    Id = Guid.NewGuid()
                },

                new Product("Iphone 15", 800.77m, 10) { Id = Guid.NewGuid() }
                );
        }); 

    }
    
}
