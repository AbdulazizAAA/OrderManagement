using MediatR;
using OrderManagement.Application.DTOs;
using System;

namespace OrderManagement.Application.Features.Customer.Orders.Queries;

public class GetOrderByIdQuery
    : IRequest<OrderDto>
{
    public Guid Id { get; set; }
}
