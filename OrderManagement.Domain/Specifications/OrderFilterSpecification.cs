using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Infrastructure.Persistence.Data.Specifications;
using System;

namespace OrderManagement.Domain.Interfaces;

public class OrderFilterSpecification : BaseSpecification<Order>
{
    public OrderFilterSpecification(
        string? searchTerm,
        OrderStatus? status,
        Guid? customerId,
        string? sortBy,
        bool sortDescending,
        int? skip = null,
        int? take = null)
    {
        AddInclude(o => o.Customer);
        AddInclude(o => o.Items);

        // Build combined criteria
        AddCriteria(o =>
            (string.IsNullOrEmpty(searchTerm) ||
                o.OrderNumber.Contains(searchTerm) ||
                o.Customer.FirstName.Contains(searchTerm) ||
                o.Customer.LastName.Contains(searchTerm) ||
                o.Customer.Email.Contains(searchTerm)) &&
            (!status.HasValue || o.Status == status.Value) &&
            (!customerId.HasValue || o.CustomerId == customerId.Value)
        );

        // Sorting
        switch (sortBy?.ToLower())
        {
            case "ordernumber": if (sortDescending) AddOrderByDescending(o => o.OrderNumber); else AddOrderBy(o => o.OrderNumber); break;
            case "total": if (sortDescending) AddOrderByDescending(o => o.TotalAmount); else AddOrderBy(o => o.TotalAmount); break;
            case "status": if (sortDescending) AddOrderByDescending(o => o.Status); else AddOrderBy(o => o.Status); break;
            default: if (sortDescending) AddOrderByDescending(o => o.OrderDate); else AddOrderByDescending(o => o.OrderDate); break;
        }

        if (skip.HasValue && take.HasValue)
            ApplyPaging(skip.Value, take.Value);
    }
}