using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Queries.GetById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(request.OrderId);
        if (order is null) return null;

        return new OrderDto(order.Id, order.OrderNumber, order.CustomerId,
            order.Customer?.FullName ?? "", order.Customer?.Email ?? "",
            order.Status, order.Status.ToString(), order.DiscountStrategy,
            order.DiscountStrategy.ToString(), order.SubTotal, order.DiscountAmount,
            order.TotalAmount, order.OrderDate, order.LastModifiedAt,
            order.Items.Select(i => new OrderItemDto(i.Id, i.OrderId, i.ProductName,
                i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList());
    }
}
