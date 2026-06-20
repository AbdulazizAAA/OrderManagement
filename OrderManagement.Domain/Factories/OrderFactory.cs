using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.Domain.Factories;

public class OrderFactory : IOrderFactory
{
    public Order Create(
        Guid customerId,
        IEnumerable<OrderItem> items)
    {
        if (!items.Any())
            throw new Exception("Order must contain items");

        var order = new Order(customerId);

        foreach (var item in items)
            order.AddItem(item);

        return order;
    }
}
