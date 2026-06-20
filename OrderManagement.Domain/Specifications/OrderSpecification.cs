using global::OrderManagement.Domain.Entities;

namespace OrderManagement.Domain.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(
        string? search,
        int page,
        int pageSize,
        string? sortBy,
        bool desc)
    {
        // FILTER / SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            AddCriteria(o =>
                o.CustomerId.ToString().Contains(search));
        }
        else
        {
            AddCriteria(o => true);
        }

        // PAGINATION
        ApplyPaging(
            (page - 1) * pageSize,
            pageSize);

        // SORTING
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            ApplySorting(sortBy, desc);
        }
        else
        {
            ApplyOrderByDescending(o => o.Created);
        }
    }

    private void ApplySorting(string sortBy, bool desc)
    {
        switch (sortBy.ToLower())
        {
            case "total":
                if (desc)
                    ApplyOrderByDescending(o => o.Discount);
                else
                    ApplyOrderBy(o => o.Discount);
                break;

            case "created":
                if (desc)
                    ApplyOrderByDescending(o => o.Created);
                else
                    ApplyOrderBy(o => o.Created);
                break;

            default:
                ApplyOrderByDescending(o => o.Created);
                break;
        }
    }
}
