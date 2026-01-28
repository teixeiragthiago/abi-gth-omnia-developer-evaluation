using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Base;

public static class CreateSaleResultMapping
{
    public static BaseSaleResult? ToResult(this Sale sale)
    {
        return new BaseSaleResult
        {
            Id =  sale.Id,
            IsCancelled =  sale.IsCancelled,
            CustomerId = sale.CustomerId,
            BranchId =  sale.BranchId,
            UpdatedAt =  sale.UpdatedAt,
            CreatedAt =   sale.CreatedAt,
            TotalAmount =  sale.TotalAmount,
            CancelledAt =   sale.CancelledAt,   
            Products = sale.Items.Any() ? sale.Items.Select(x => new SaleProductResult
            {
                Id =  x.Id,
                SaleId =  x.SaleId,
                ProductId = x.ProductId,
                UnitPrice =  x.UnitPrice,
                Quantity =   x.Quantity,
                DiscountAmount =  x.DiscountAmount,
                DiscountPercentage =   x.DiscountPercentage,
                CreatedAt =   x.CreatedAt,
                UpdatedAt =  x.UpdatedAt,
                TotalAmount =  x.TotalAmount,
            }) :Enumerable.Empty<SaleProductResult>()
        };
    }
}