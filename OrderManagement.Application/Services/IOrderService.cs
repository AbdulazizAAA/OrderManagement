using OrderManagement.Application.Features.Customer.Orders.Commands;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(
        CreateOrderCommand command);

    //Task DeleteOrderAsync(Guid id);

    //Task UpdateOrderAsync(
    //    UpdateOrderCommand command);
}
