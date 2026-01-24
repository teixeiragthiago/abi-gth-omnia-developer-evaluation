namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class SaleResponse
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    // public SaleStatus Status { get; set; } //TODO
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<SaleItemResponse> Items { get; set; } = [];
}