using Ambev.DeveloperEvaluation.Application.Sales.Base;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;

public class IncludeSaleItemCommand : IRequest<BaseSaleResult>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid SaleId { get; set; }
}