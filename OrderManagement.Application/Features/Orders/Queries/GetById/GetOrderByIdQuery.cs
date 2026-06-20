using MediatR;
using OrderManagement.Application.Common;
using System;

namespace OrderManagement.Application.Features.Orders.Queries.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto?>;

