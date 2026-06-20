using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
{
    private readonly IUnitOfWork unitOfWork;

    public UpdateOrderStatusHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetOrderWithItemsAsync(request.OrderId)
            ?? throw new KeyNotFoundException($"Order {request.OrderId} not found.");

        order.UpdateStatus(request.NewStatus);
        unitOfWork.Orders.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new OrderDto(order.Id, order.OrderNumber, order.CustomerId,
            order.Customer?.FullName ?? "", order.Customer?.Email ?? "",
            order.Status, order.Status.ToString(), order.DiscountStrategy,
            order.DiscountStrategy.ToString(), order.SubTotal, order.DiscountAmount,
            order.TotalAmount, order.OrderDate, order.LastModifiedAt,
            order.Items.Select(i => new OrderItemDto(i.Id, i.OrderId, i.ProductName,
                i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList());
    }
}