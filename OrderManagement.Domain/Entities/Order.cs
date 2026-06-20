using OrderManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.Domain.Entities;

public class Order : AuditableBaseEntity
{
    private readonly List<OrderItem> items = [];

    private Order()
    {
    }

    internal Order(Guid customerId)
    {
        Id = Guid.NewGuid();

        CustomerId = customerId;
    }

    public Guid CustomerId { get; private set; }

    public decimal Discount { get; private set; }

    public IReadOnlyCollection<OrderItem>
        Items => items.AsReadOnly();

    public decimal SubTotal =>
        items.Sum(x => x.Total);

    public decimal Total =>
        SubTotal - Discount;

    public void AddItem(OrderItem item)
    {
        items.Add(item);
    }

    public void ApplyDiscount(decimal discount)
    {
        Discount = discount;
    }
}