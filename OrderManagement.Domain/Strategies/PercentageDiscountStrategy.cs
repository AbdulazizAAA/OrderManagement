using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Strategies;

public class PercentageDiscountStrategy
    : IDiscountStrategy
{
    public string Name => StrategiesNames.PercentageDiscountStrategy;

    public decimal Calculate(decimal total)
    {
        return total * 0.10m;
    }
}
