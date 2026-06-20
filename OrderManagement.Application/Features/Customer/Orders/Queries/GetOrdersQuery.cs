using MediatR;
using OrderManagement.Application.DTOs;
using System.Collections.Generic;


namespace OrderManagement.Application.Features.Customer.Orders.Queries;

public class GetOrdersQuery
    : IRequest<List<OrderDto>>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
}
