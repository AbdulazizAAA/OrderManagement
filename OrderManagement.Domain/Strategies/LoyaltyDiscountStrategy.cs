using System;

namespace OrderManagement.Domain.Strategies;

public class LoyaltyDiscountStrategy : IDiscountStrategy
{
    public string Name => "Loyalty Discount (20%)";
    public Domain.Enums.DiscountStrategy StrategyType => Domain.Enums.DiscountStrategy.LoyaltyDiscount;
    public decimal Calculate(decimal subTotal, int itemCount) => Math.Round(subTotal * 0.20m, 2);
}
