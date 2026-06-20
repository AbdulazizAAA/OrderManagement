using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Strategies;

public class PremiumCustomerDiscountStrategy
    : IDiscountStrategy
{
    public string Name => StrategiesNames.PremiumCustomerDiscountStrategy;

    public decimal Calculate(decimal total)
    {
        return total * 0.20m;
    }
}
