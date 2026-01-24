using Ambev.DeveloperEvaluation.Application.Sales.Base;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<SaleResult>
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public IEnumerable<SaleItemDto> Items { get; set; } = Enumerable.Empty<SaleItemDto>();
}