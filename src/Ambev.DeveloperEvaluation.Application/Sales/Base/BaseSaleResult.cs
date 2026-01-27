namespace Ambev.DeveloperEvaluation.Application.Sales.Base;

public record BaseSaleResult
{
 
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? CancelledAt { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<SaleItemResult> Items { get; set; } = [];
}