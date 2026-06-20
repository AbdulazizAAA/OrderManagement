using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderManagement.Domain.Strategies.Base;

public class DiscountStrategyFactory
    : IDiscountStrategyFactory
{
    private readonly IEnumerable<IDiscountStrategy>
        strategies;

    public DiscountStrategyFactory(
        IEnumerable<IDiscountStrategy> strategies)
    {
        this.strategies = strategies;
    }

    public IDiscountStrategy Get(string strategy)
    {
        return strategies.First(x =>
            x.Name.Equals(strategy,
            StringComparison.OrdinalIgnoreCase));
    }
}
