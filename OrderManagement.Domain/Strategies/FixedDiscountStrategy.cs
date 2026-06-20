using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Strategies;

public class FixedDiscountStrategy
    : IDiscountStrategy
{
    public string Name => StrategiesNames.FixedDiscountStrategy;

    public decimal Calculate(decimal total)
    {
        return 50;
    }
}
