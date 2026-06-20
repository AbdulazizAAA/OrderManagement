namespace OrderManagement.Application.Features.Orders.Commands.Create;

public record CreateOrderItemDto(
    string ProductName,
    string ProductCode,
    int Quantity,
    decimal UnitPrice
);
