using System;

namespace OrderManagement.Domain.Strategies;

public class BulkDiscountStrategy : IDiscountStrategy
{
    public string Name => "Bulk Discount (10+ items = 15%)";
    public Domain.Enums.DiscountStrategy StrategyType => Domain.Enums.DiscountStrategy.BulkDiscount;

    public decimal Calculate(decimal subTotal, int itemCount) =>
        itemCount >= 10 ? Math.Round(subTotal * 0.15m, 2) : 0m;
}
