using System;

namespace OrderManagement.Application.Common;

public record OrderItemDto(
    Guid Id,
    Guid OrderId,
    string ProductName,
    string ProductCode,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
