using OrderManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Interfaces;

public interface IOrderRepository
    : IRepository<Order>
{
    Task<List<Order>> ListAsync(
        ISpecification<Order> specification);
}
