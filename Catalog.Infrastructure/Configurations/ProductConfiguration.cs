

using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Configurations;

public class ProductConfiguration:IEntityTypeConfiguration<Product>
{
   

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.HasData(
            new Product("Lenovo", 999.99m, 10)
            {
                Id = Guid.NewGuid()
            },

            new Product("Iphone 15", 800.77m, 10) { Id = Guid.NewGuid() }
            );
    }
}
