namespace OrderManagement.Frontend.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = "";
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public int Status { get; set; } = 0;
    public string StatusName { get; set; } = "";
    public int DiscountStrategy { get; set; }
    public string DiscountStrategyName { get; set; } = "";
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
}
