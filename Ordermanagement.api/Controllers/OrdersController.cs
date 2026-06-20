using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Common;
using OrderManagement.Application.Features.Orders.Commands.Calculate;
using OrderManagement.Application.Features.Orders.Commands.Create;
using OrderManagement.Application.Features.Orders.Commands.Delete;
using OrderManagement.Application.Features.Orders.Commands.UpdateOrderStatus;
using OrderManagement.Application.Features.Orders.Queries.GetAll;
using OrderManagement.Application.Features.Orders.Queries.GetById;
using OrderManagement.Application.Features.Orders.Queries.GetDisscount;
using OrderManagement.Domain.Enums;

namespace OrderManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get paginated, filtered, sorted orders</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OrderDto>), 200)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] string? search,
        [FromQuery] OrderStatus? status,
        [FromQuery] Guid? customerId,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetOrdersQuery(
            search, status, customerId, sortBy, sortDesc, page, pageSize));
        return Ok(result);
    }

    /// <summary>Get single order with items</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));
        return order is null ? NotFound(new { message = $"Order {id} not found." }) : Ok(order);
    }

    /// <summary>Create a new order with items</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
    }

    /// <summary>Update order status</summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateOrderStatusCommand(id, request.Status));
        return Ok(result);
    }

    /// <summary>Delete an order</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }

    /// <summary>Preview discount amount before creating order</summary>
    [HttpPost("discount/preview")]
    [ProducesResponseType(typeof(DiscountPreviewDto), 200)]
    public async Task<IActionResult> PreviewDiscount([FromBody] DiscountPreviewRequest request)
    {
        var result = await _mediator.Send(new CalculateDiscountCommand(
            request.Strategy, request.SubTotal, request.ItemCount));
        return Ok(result);
    }

    /// <summary>Get available discount strategies</summary>
    [HttpGet("discount/strategies")]
    [ProducesResponseType(typeof(List<DiscountStrategyDto>), 200)]
    public async Task<IActionResult> GetDiscountStrategies()
    {
        var result = await _mediator.Send(new GetDiscountStrategiesQuery());
        return Ok(result);
    }
}

public record UpdateStatusRequest(OrderStatus Status);
public record DiscountPreviewRequest(DiscountStrategy Strategy, decimal SubTotal, int ItemCount);