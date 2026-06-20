namespace OrderManagement.Domain.Strategies;

public interface IDiscountStrategy
{
    string Name { get; }
    Domain.Enums.DiscountStrategy StrategyType { get; }
    decimal Calculate(decimal subTotal, int itemCount);
}
