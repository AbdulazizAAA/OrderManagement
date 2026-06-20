using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Customer.Orders.Queries;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order =
            await _repository.GetByIdAsync(request.Id);

        return new OrderDto
        {
            Id = order!.Id,
            CustomerId = order.CustomerId,
            Total = order.Total,
            Discount = order.Discount
        };
    }
}
