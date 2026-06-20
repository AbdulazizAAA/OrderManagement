using MediatR;
using System;

namespace OrderManagement.Application.Features.Customer.Orders.Commands;

public class DeleteOrderCommand : IRequest
{
    public Guid Id { get; set; }
}
