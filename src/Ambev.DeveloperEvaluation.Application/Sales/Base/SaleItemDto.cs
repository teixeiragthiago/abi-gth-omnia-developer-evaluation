namespace Ambev.DeveloperEvaluation.Application.Sales.Base;

public record SaleItemDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}