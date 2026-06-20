using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OrderManagement.Application.Common;

public record OrderDto(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    OrderStatus Status,
    string StatusName,
    DiscountStrategy DiscountStrategy,
    string DiscountStrategyName,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal TotalAmount,
    DateTime OrderDate,
    DateTime? LastModifiedAt,
    List<OrderItemDto> Items
);
