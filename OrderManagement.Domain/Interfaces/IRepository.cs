using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Interfaces;

public interface IRepository<T>
    where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task<List<T>> ListAsync(
        ISpecification<T> specification);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);
}
