using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithItemsAsync(Guid id);
    Task<IReadOnlyList<Order>> GetOrdersWithDetailsAsync(ISpecification<Order> spec);
}
