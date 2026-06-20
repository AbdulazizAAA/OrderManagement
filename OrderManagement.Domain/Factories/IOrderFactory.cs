using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace OrderManagement.Domain.Factories;

public interface IOrderFactory
{
    Order Create(
        Guid customerId,
        IEnumerable<OrderItem> items);
}
