using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Enums;
using System;

namespace OrderManagement.Application.Features.Orders.Queries.GetAll;

public record GetOrdersQuery(
    string? SearchTerm = null,
    OrderStatus? Status = null,
    Guid? CustomerId = null,
    string? SortBy = null,
    bool SortDescending = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderDto>>;
