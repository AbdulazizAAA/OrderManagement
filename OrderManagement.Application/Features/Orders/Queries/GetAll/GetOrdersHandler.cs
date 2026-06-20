using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Queries.GetAll;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<PagedResult<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var spec = new OrderFilterSpecification(
            request.SearchTerm, request.Status, request.CustomerId,
            request.SortBy, request.SortDescending, (request.PageNumber - 1) * request.PageSize, request.PageSize);

        var countSpec = new OrderFilterSpecification(
            request.SearchTerm, request.Status, request.CustomerId,
            request.SortBy, request.SortDescending, (request.PageNumber - 1) * request.PageSize, request.PageSize);

        var totalCount = await _unitOfWork.Orders.CountAsync(countSpec);

        var pagedSpec = new OrderFilterSpecification(
            request.SearchTerm, request.Status, request.CustomerId,
            request.SortBy, request.SortDescending,
            (request.PageNumber - 1) * request.PageSize, request.PageSize);

        var orders = await _unitOfWork.Orders.GetOrdersWithDetailsAsync(pagedSpec);

        var dtos = orders.Select(order => new OrderDto(
            order.Id, order.OrderNumber, order.CustomerId,
            order.Customer?.FullName ?? "", order.Customer?.Email ?? "",
            order.Status, order.Status.ToString(), order.DiscountStrategy,
            order.DiscountStrategy.ToString(), order.SubTotal, order.DiscountAmount,
            order.TotalAmount, order.OrderDate, order.LastModifiedAt,
            order.Items.Select(i => new OrderItemDto(i.Id, i.OrderId, i.ProductName,
                i.ProductCode, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList()
        )).ToList();

        return new PagedResult<OrderDto>(dtos, totalCount, request.PageNumber,
            request.PageSize, (int)Math.Ceiling(totalCount / (double)request.PageSize));
    }
}
