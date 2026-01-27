using Ambev.DeveloperEvaluation.Application.Sales.Base;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleCommand : IRequest<BaseSaleResult>
{
    public Guid Id { get; set; }
}