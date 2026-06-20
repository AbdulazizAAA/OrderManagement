using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Strategies.Base;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders.Commands.Calculate;

public class CalculateDiscountHandler : IRequestHandler<CalculateDiscountCommand, DiscountPreviewDto>
{
    private readonly IDiscountStrategyResolver resolver;

    public CalculateDiscountHandler(IDiscountStrategyResolver resolver) => this.resolver = resolver;

    public Task<DiscountPreviewDto> Handle(CalculateDiscountCommand request, CancellationToken cancellationToken)
    {
        var strategy = resolver.Resolve(request.Strategy);
        var discount = strategy.Calculate(request.SubTotal, request.ItemCount);
        return Task.FromResult(new DiscountPreviewDto(
            strategy.Name, request.SubTotal, discount, request.SubTotal - discount));
    }
}