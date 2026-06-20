using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Features.Orders.Commands.Create;

public record CreateOrderCommand(
    Guid CustomerId,
    DiscountStrategy DiscountStrategy,
    List<CreateOrderItemDto> Items
) : IRequest<OrderDto>;

