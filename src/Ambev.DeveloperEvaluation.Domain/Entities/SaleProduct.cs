using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleProduct : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    public SaleProduct(Guid productId,
        decimal unitPrice,
        int quantity,
        decimal discountPercentage)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        DiscountPercentage = discountPercentage;
        DiscountAmount = CalculateDiscountAmount();
        TotalAmount = CalculateTotalAmount();
        CreatedAt = DateTime.UtcNow;
    }
    
    
    private decimal CalculateDiscountAmount()
    {
        return (Quantity * UnitPrice) * DiscountPercentage;
    }

    private decimal CalculateTotalAmount()
    {
        return (Quantity * UnitPrice) - DiscountAmount;
    }
}