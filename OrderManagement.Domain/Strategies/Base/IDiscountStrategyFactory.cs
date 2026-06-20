namespace OrderManagement.Domain.Strategies.Base;

public interface IDiscountStrategyFactory
{
    IDiscountStrategy Get(string strategy);
}
