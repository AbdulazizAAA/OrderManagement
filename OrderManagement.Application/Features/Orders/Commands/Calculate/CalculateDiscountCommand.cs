using MediatR;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Features.Orders.Commands.Calculate;

public record CalculateDiscountCommand(
    DiscountStrategy Strategy,
    decimal SubTotal,
    int ItemCount
) : IRequest<DiscountPreviewDto>;
