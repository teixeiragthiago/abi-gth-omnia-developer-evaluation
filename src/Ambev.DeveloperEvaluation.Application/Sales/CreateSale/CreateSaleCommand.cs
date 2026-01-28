using Ambev.DeveloperEvaluation.Application.Sales.Base;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<BaseSaleResult>
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public IEnumerable<SaleProductDto> Items { get; set; } = Enumerable.Empty<SaleProductDto>();
}