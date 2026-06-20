using System;
using System.Collections.Generic;

namespace OrderManagement.Domain.Interfaces;

public interface ISpecification<T>
{
    System.Linq.Expressions.Expression<Func<T, bool>> Criteria { get; }
    List<System.Linq.Expressions.Expression<Func<T, object>>> Includes { get; }
    System.Linq.Expressions.Expression<Func<T, object>>? OrderBy { get; }
    System.Linq.Expressions.Expression<Func<T, object>>? OrderByDescending { get; }
    int? Skip { get; }
    int? Take { get; }
    bool IsPagingEnabled { get; }
}
