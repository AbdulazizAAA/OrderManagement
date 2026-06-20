using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Factories;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Strategies.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Customer.Orders.Commands;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderFactory _factory;
    private readonly IDiscountStrategyFactory _strategyFactory;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork,
        IOrderFactory factory,
        IDiscountStrategyFactory strategyFactory)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _factory = factory;
        _strategyFactory = strategyFactory;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var items = request.Items.Select(x =>
            new OrderItem(
                x.ProductName,
                x.UnitPrice,
                x.Quantity));

        var order = _factory.Create(
            request.CustomerId,
            items);

        var strategy =
            _strategyFactory.Get(
                request.DiscountStrategy);

        var discount =
            strategy.Calculate(order.SubTotal);

        order.ApplyDiscount(discount);

        await _repository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}