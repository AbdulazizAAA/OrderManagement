using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Domain.Strategies;

public class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "No Discount";
    public Domain.Enums.DiscountStrategy StrategyType => Domain.Enums.DiscountStrategy.None;
    public decimal Calculate(decimal subTotal, int itemCount) => 0m;
}
