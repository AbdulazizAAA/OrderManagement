using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Features.Customer.Orders.Queries;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository repository;

    public GetOrdersQueryHandler(
        IOrderRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<OrderDto>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new OrderSpecification(
            request.Search,
            request.Page,
            request.PageSize,null,true);

        var orders =
            await repository.ListAsync(spec);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            Total = o.Total,
            Discount = o.Discount
        }).ToList();
    }
}
