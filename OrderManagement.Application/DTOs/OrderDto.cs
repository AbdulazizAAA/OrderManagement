using System;

namespace OrderManagement.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public decimal Total { get; set; }

    public decimal Discount { get; set; }
}
