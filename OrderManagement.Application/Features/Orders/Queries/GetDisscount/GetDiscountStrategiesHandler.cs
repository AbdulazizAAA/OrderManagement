using MediatR;
using OrderManagement.Domain.Strategies.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Queries.GetDisscount;

public class GetDiscountStrategiesHandler : IRequestHandler<GetDiscountStrategiesQuery, List<DiscountStrategyDto>>
{
    private readonly IDiscountStrategyResolver resolver;

    public GetDiscountStrategiesHandler(IDiscountStrategyResolver resolver) => this.resolver = resolver;

    public Task<List<DiscountStrategyDto>> Handle(GetDiscountStrategiesQuery request, CancellationToken cancellationToken)
    {
        var strategies = resolver.GetAvailable()
            .Select(s => new DiscountStrategyDto((int)s.Key, s.Name))
            .ToList();
        return Task.FromResult(strategies);
    }
}
