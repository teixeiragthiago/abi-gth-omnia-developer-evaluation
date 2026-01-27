using Ambev.DeveloperEvaluation.Application.Sales.Base;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleCommand : IRequest<BaseSaleResult>
{
    public Guid Id { get; set; }
}