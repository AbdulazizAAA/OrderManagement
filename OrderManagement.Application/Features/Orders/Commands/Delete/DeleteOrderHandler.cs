using MediatR;
using OrderManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Commands.Delete;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly IUnitOfWork unitOfWork;

    public DeleteOrderHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(request.OrderId)
            ?? throw new KeyNotFoundException($"Order {request.OrderId} not found.");

        unitOfWork.Orders.Delete(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
