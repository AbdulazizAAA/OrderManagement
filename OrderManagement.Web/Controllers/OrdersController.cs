using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Features.Customer.Orders.Commands;
using OrderManagement.Application.Features.Customer.Orders.Queries;

namespace OrderManagement.Web.Controllers;

public class OrdersController : Controller
{
    private readonly IMediator mediator;

    public OrdersController(
        IMediator mediator)
    {
        this.mediator = mediator;
    }


    public async Task<IActionResult> Index()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult>
        Create(CreateOrderCommand command)
    {
        var id =
            await mediator.Send(command);

        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult>
        Get([FromQuery] GetOrdersQuery query)
    {
        return Ok(
            await mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetById(Guid id)
    {
        return Ok(
            //await _mediator.Send(
            //    new GetOrderByIdQuery(id))
            );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult>
        Update(
            Guid id
           /* ,UpdateOrderCommand command*/)
    {
        //await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        Delete(Guid id)
    {
        //await _mediator.Send(
        //    new DeleteOrderCommand(id));

        return NoContent();
    }
}
