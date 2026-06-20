using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Strategies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Factories;

public class OrderFactory : IOrderFactory
{
    private readonly IDiscountStrategyResolver discountResolver;
    private static int orderCounter = 0;

    public OrderFactory(IDiscountStrategyResolver discountResolver)
    {
        this.discountResolver = discountResolver;
    }

    public Task<Order> CreateOrderAsync(Guid customerId, DiscountStrategy discountStrategy,
        IEnumerable<(string ProductName, string ProductCode, int Quantity, decimal UnitPrice)> items)
    {
        var itemList = items.ToList();
        if (!itemList.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var orderId = Guid.NewGuid();
        var orderNumber = GenerateOrderNumber();
        var order = new Order(orderId, orderNumber, customerId, discountStrategy);

        foreach (var (productName, productCode, quantity, unitPrice) in itemList)
        {
            var item = new OrderItem(orderId, productName, productCode, quantity, unitPrice);
            order.AddItem(item);
        }

        // Apply discount using strategy
        var strategy = discountResolver.Resolve(discountStrategy);
        var totalQty = itemList.Sum(i => i.Quantity);
        var discount = strategy.Calculate(order.SubTotal, totalQty);
        order.ApplyDiscount(discount);

        return Task.FromResult(order);
    }

    private static string GenerateOrderNumber()
    {
        var counter = Interlocked.Increment(ref orderCounter);
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{counter:D4}";
    }
}
