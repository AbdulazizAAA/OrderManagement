using MediatR;
using System;

namespace OrderManagement.Application.Features.Orders.Commands.Delete;

public record DeleteOrderCommand(Guid OrderId) : IRequest<bool>;

