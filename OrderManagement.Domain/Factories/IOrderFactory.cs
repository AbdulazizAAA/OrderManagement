using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Factories;

public interface IOrderFactory
{
    Task<Order> CreateOrderAsync(Guid customerId, DiscountStrategy discountStrategy,
    IEnumerable<(string ProductName, string ProductCode, int Quantity, decimal UnitPrice)> items);
}