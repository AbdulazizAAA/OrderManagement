using MediatR;
using OrderManagement.Application.Features.Customer.Orders.Commands;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services;

public class OrderService : IOrderService
{
    private readonly IMediator mediator;

    public OrderService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<Guid> CreateOrderAsync(
        CreateOrderCommand command)
    {
        return await mediator.Send(command);
    }

    //public async Task UpdateOrderAsync(
    //    UpdateOrderCommand command)
    //{
    //    await mediator.Send(command);
    //}

    //public async Task DeleteOrderAsync(Guid id)
    //{
    //    await mediator.Send(
    //        new DeleteOrderCommand(id));
    //}
}