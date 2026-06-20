using System;

namespace OrderManagement.Domain.Entities;

public class OrderItem 
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public string ProductCode { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    private OrderItem() { }

    internal OrderItem(Guid orderId, string productName, string productCode, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        ProductCode = productCode ?? throw new ArgumentNullException(nameof(productCode));
        Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive", nameof(quantity));
        UnitPrice = unitPrice > 0 ? unitPrice : throw new ArgumentException("Unit price must be positive", nameof(unitPrice));
    }

    public void Update(string productName, string productCode, int quantity, decimal unitPrice)
    {
        ProductName = productName;
        ProductCode = productCode;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}