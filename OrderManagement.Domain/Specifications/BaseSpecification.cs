using global::OrderManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrderManagement.Domain.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    public List<Expression<Func<T, object>>> Includes { get; } = new();

    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

    public int Skip { get; protected set; }

    public int Take { get; protected set; }

    public bool IsPagingEnabled { get; protected set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
        => Criteria = criteria;

    protected void AddInclude(Expression<Func<T, object>> include)
        => Includes.Add(include);

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
        => OrderBy = orderBy;

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc)
        => OrderByDescending = orderByDesc;
}
