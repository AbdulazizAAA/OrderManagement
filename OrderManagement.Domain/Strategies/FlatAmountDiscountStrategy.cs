namespace OrderManagement.Domain.Strategies;

public class FlatAmountDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _amount;
    public string Name => $"${_amount} Flat Discount";
    public Domain.Enums.DiscountStrategy StrategyType => Domain.Enums.DiscountStrategy.FlatAmount;

    public FlatAmountDiscountStrategy(decimal amount = 20m) => _amount = amount;
    public decimal Calculate(decimal subTotal, int itemCount) => subTotal >= _amount ? _amount : 0m;
}