using Microsoft.EntityFrameworkCore.Storage;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.Persistence.Contexts;
using OrderManagement.Infrastructure.Persistence.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Persistence.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private IDbContextTransaction? transaction;
    private bool _disposed;

    private IOrderRepository? orders;
    private ICustomerRepository? customers;

    public UnitOfWork(ApplicationDbContext context) => this.context = context;

    public IOrderRepository Orders => orders ??= new OrderRepository(context);
    public ICustomerRepository Customers => customers ??= new CustomerRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync() =>
        transaction = await context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        if (transaction != null)
        {
            await transaction.CommitAsync();
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync();
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            transaction?.Dispose();
            context.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
