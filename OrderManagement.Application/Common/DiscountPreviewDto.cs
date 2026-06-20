namespace OrderManagement.Application.Common;

public record DiscountPreviewDto(
    string StrategyName,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal TotalAmount
);
