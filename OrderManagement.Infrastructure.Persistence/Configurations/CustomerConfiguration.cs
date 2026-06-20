using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;
using System;
using System.Reflection.Emit;

namespace OrderManagement.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(
        EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerId);

        var customerId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var customerId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.HasData(
            new { Id = customerId1, FirstName = "Abdelaziz", LastName = "Ahmed", Email = "Abdelaziz@gmail.com", Phone = "555-0101", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = customerId2, FirstName = "Khaled", LastName = "Mohmaed", Email = "Khaled@example.com", Phone = "555-0102", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
