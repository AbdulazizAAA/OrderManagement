using OrderManagement.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Domain.Entities
{
    public class OrderItem : AuditableBaseEntity
    {
        public Guid Id { get; private set; }

        public string ProductName { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public decimal Total => UnitPrice * Quantity;

        private OrderItem() { }

        public OrderItem(string productName,
            decimal unitPrice,
            int quantity)
        {
            Id = Guid.NewGuid();
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}