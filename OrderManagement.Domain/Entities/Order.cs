using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Events;
using OrderManagement.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public DiscountStrategy DiscountStrategy { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal SubTotal => _items.Sum(i => i.TotalPrice);
    public decimal TotalAmount => SubTotal - DiscountAmount;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // EF Core constructor
    private Order() { }

    internal Order(Guid id, string orderNumber, Guid customerId, DiscountStrategy discountStrategy)
    {
        Id = id;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        DiscountStrategy = discountStrategy;
        OrderDate = DateTime.UtcNow;
        _domainEvents.Add(new OrderCreatedEvent(id));
    }

    internal void AddItem(OrderItem item) => _items.Add(item);

    public void ApplyDiscount(decimal amount)
    {
        DiscountAmount = amount;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}