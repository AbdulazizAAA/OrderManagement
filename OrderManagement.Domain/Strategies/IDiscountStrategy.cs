namespace OrderManagement.Domain.Strategies;

public interface IDiscountStrategy
{
    string Name { get; }
    decimal Calculate(decimal total);
}
