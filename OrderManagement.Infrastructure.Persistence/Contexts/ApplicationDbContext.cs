using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Common;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Persistence.Contexts;

public sealed class ApplicationDbContext : DbContext,
    IUnitOfWork
{
    private readonly IDateTimeService dateTimeService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeService dateTimeService)
        : base(options)
    {
        this.dateTimeService = dateTimeService;
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        foreach (var entry in ChangeTracker
                     .Entries<AuditableBaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created =
                    dateTimeService.NowUtc;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified =
                    dateTimeService.NowUtc;
            }
        }
    }

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}