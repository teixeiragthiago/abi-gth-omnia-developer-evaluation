namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public Guid CustomerId { get; init; }
    public Guid BranchId { get; init; }
    public IEnumerable<CreateSaleItemRequest> Items { get; set; } = Enumerable.Empty<CreateSaleItemRequest>();
}