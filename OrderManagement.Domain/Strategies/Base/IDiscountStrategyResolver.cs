using System.Collections.Generic;

namespace OrderManagement.Domain.Strategies.Base;

public interface IDiscountStrategyResolver
{
    IDiscountStrategy Resolve(Domain.Enums.DiscountStrategy strategy);
    IReadOnlyList<(Domain.Enums.DiscountStrategy Key, string Name)> GetAvailable();
}
