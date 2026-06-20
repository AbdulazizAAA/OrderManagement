using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IUnitOfWork uow;

    public CustomersController(IUnitOfWork uow) => this.uow = uow;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await uow.Customers.ListAllAsync();
        var dtos = customers.Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName, c.FullName, c.Email, c.Phone, c.CreatedAt)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var c = await uow.Customers.GetByIdAsync(id);
        if (c is null) return NotFound();
        return Ok(new CustomerDto(c.Id, c.FirstName, c.LastName, c.FullName, c.Email, c.Phone, c.CreatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest req)
    {
        var customer = new Domain.Entities.Customer(req.FirstName, req.LastName, req.Email, req.Phone);
        await uow.Customers.AddAsync(customer);
        await uow.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = customer.Id },
            new CustomerDto(customer.Id, customer.FirstName, customer.LastName, customer.FullName, customer.Email, customer.Phone, customer.CreatedAt));
    }
}

public record CreateCustomerRequest(string FirstName, string LastName, string Email, string Phone);