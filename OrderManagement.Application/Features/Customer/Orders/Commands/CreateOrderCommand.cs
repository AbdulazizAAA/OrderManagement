using MediatR;
using OrderManagement.Application.DTOs;
using System;
using System.Collections.Generic;

namespace OrderManagement.Application.Features.Customer.Orders.Commands;

public class CreateOrderCommand : IRequest<Guid>
{
    public Guid CustomerId { get; set; }

    public string DiscountStrategy { get; set; } = string.Empty;

    public List<OrderItemDto> Items { get; set; } = [];
}
