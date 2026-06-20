using OrderManagement.Domain.Entities;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}
