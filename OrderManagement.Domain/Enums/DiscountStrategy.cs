namespace OrderManagement.Domain.Enums;

public enum DiscountStrategy
{
    None = 0,
    Percentage = 1,
    FlatAmount = 2,
    BulkDiscount = 3,
    LoyaltyDiscount = 4
}