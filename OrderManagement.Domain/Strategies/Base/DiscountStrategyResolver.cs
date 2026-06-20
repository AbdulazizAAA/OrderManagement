using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderManagement.Domain.Strategies.Base;

public class DiscountStrategyResolver : IDiscountStrategyResolver
{
    private readonly Dictionary<Domain.Enums.DiscountStrategy, IDiscountStrategy> strategies;

    public DiscountStrategyResolver()
    {
        strategies = new()
        {
            [Domain.Enums.DiscountStrategy.None] = new NoDiscountStrategy(),
            [Domain.Enums.DiscountStrategy.Percentage] = new PercentageDiscountStrategy(),
            [Domain.Enums.DiscountStrategy.FlatAmount] = new FlatAmountDiscountStrategy(),
            [Domain.Enums.DiscountStrategy.BulkDiscount] = new BulkDiscountStrategy(),
            [Domain.Enums.DiscountStrategy.LoyaltyDiscount] = new LoyaltyDiscountStrategy(),
        };
    }

    public IDiscountStrategy Resolve(Domain.Enums.DiscountStrategy strategy) =>
        strategies.TryGetValue(strategy, out var s) ? s : strategies[Domain.Enums.DiscountStrategy.None];

    public IReadOnlyList<(Domain.Enums.DiscountStrategy Key, string Name)> GetAvailable() =>
        strategies.Select(kvp => (kvp.Key, kvp.Value.Name)).ToList();
}