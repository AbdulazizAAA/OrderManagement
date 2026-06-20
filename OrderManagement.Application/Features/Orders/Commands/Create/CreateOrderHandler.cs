using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Factories;
using OrderManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Commands.Create;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IOrderFactory orderFactory;

    public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderFactory orderFactory)
    {
        this.unitOfWork = unitOfWork;
        this.orderFactory = orderFactory;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(request.CustomerId)
            ?? throw new KeyNotFoundException($"Customer {request.CustomerId} not found.");

        var items = request.Items.Select(i =>
            (i.ProductName, i.ProductCode, i.Quantity, i.UnitPrice));

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await orderFactory.CreateOrderAsync(request.CustomerId, request.DiscountStrategy, items);
            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            return MapToDto(order, customer.FullName, customer.Email);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private static OrderDto MapToDto(Domain.Entities.Order order, string customerName, string email) =>
        new(order.Id, order.OrderNumber, order.CustomerId, customerName, email,
            order.Status, order.Status.ToString(), order.DiscountStrategy,
            order.DiscountStrategy.ToString(), order.SubTotal, order.DiscountAmount,
            order.TotalAmount, order.OrderDate, order.LastModifiedAt,
            order.Items.Select(i => new OrderItemDto(i.Id, i.OrderId, i.ProductName,
                i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList());
}
