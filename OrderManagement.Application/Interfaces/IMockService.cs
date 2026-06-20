using OrderManagement.Domain.Entities;
using System.Collections.Generic;

namespace OrderManagement.Application.Interfaces
{
    public interface IMockService
    {
        List<OrderItem> GetPositions(int rowCount);

        List<Customer> GetEmployees(int rowCount);

        List<OrderItem> SeedPositions(int rowCount);
    }
}