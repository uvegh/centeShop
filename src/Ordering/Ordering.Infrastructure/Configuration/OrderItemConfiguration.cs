

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Configuration;


public class OrderItemConfiguration:IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> modelBuilder) {
        modelBuilder.ToTable("OrderItem");
        modelBuilder.HasKey(p => p.Id);
        modelBuilder.Property(p => p.ProductName).IsRequired();
        modelBuilder.Property(p => p.Quantity).IsRequired();
        modelBuilder.Property(p => p.UnitPrice).IsRequired();
        modelBuilder.Property(p => p.ProductId);
    }
}
