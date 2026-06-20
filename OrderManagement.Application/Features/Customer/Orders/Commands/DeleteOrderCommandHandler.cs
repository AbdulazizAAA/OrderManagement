//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OrderManagement.Application.Features.Customer.Orders.Commands;



//public class DeleteOrderCommandHandler
//    : IRequestHandler<DeleteOrderCommand>
//{
//    private readonly IOrderRepository _repository;
//    private readonly IUnitOfWork _unitOfWork;

//    public DeleteOrderCommandHandler(
//        IOrderRepository repository,
//        IUnitOfWork unitOfWork)
//    {
//        _repository = repository;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task Handle(
//        DeleteOrderCommand request,
//        CancellationToken cancellationToken)
//    {
//        var order =
//            await _repository.GetByIdAsync(request.Id);

//        if (order is null)
//            return;

//        _repository.Delete(order);

//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//    }
//}