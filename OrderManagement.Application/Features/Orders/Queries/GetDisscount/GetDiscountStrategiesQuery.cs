using MediatR;
using System.Collections.Generic;

namespace OrderManagement.Application.Features.Orders.Queries.GetDisscount;

public record GetDiscountStrategiesQuery() : IRequest<List<DiscountStrategyDto>>;
