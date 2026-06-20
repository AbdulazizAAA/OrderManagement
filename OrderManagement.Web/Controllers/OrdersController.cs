using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}
