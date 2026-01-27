namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.IncludeSaleItem;

public class IncludeSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}