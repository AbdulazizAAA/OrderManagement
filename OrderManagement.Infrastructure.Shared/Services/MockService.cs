using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.API.Infrastructure.Shared.Mock;
using System.Collections.Generic;

namespace OrderManagement.API.Infrastructure.Shared.Services
{
    public class MockService : IMockService
    {
        public List<OrderItem> GetPositions(int rowCount)
        {
            var positionFaker = new PositionInsertBogusConfig();
            return positionFaker.Generate(rowCount);
        }

        public List<Customer> GetEmployees(int rowCount)
        {
            var positionFaker = new EmployeeBogusConfig();
            return positionFaker.Generate(rowCount);
        }

        public List<OrderItem> SeedPositions(int rowCount)
        {
            var seedPositionFaker = new PositionSeedBogusConfig();
            return seedPositionFaker.Generate(rowCount);
        }
    }
}