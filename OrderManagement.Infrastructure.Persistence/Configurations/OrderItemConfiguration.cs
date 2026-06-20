using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;


namespace OrderManagement.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(
        EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.HasOne<Order>()
            .WithMany(x => x.Items)
            .HasForeignKey("OrderId");
    }
}