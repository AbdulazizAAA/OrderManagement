using System;

namespace OrderManagement.Domain.Strategies;

public class PercentageDiscountStrategy
    : IDiscountStrategy
{
    private readonly decimal _percentage;
    public Domain.Enums.DiscountStrategy StrategyType => Domain.Enums.DiscountStrategy.Percentage;

    public string Name => "Percentage";

    public PercentageDiscountStrategy(decimal percentage = 10m) => _percentage = percentage;
    public decimal Calculate(decimal subTotal, int itemCount) => Math.Round(subTotal * (_percentage / 100), 2);
}
