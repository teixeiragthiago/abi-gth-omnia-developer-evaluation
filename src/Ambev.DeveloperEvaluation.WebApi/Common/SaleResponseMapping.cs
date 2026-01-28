using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public static class SaleResponseMapping
{
    public static SaleResponse ToResponse(this BaseSaleResult saleResult)
    {
        return new SaleResponse
        {
            Id = saleResult.Id,
            BranchId =  saleResult.BranchId,
            CustomerId =  saleResult.CustomerId,
            CreatedAt = saleResult.CreatedAt,
            UpdatedAt = saleResult.UpdatedAt,
            CancelledAt =  saleResult.CancelledAt,
            IsCancelled = saleResult.IsCancelled,
            TotalAmount =  saleResult.TotalAmount,
            Products = saleResult.Products.Any() ? saleResult.Products.Select(p => new SaleProductResponse
            {
                Id =  p.Id,
                SaleId =  p.SaleId,
                ProductId = p.ProductId,
                Quantity =   p.Quantity,
                UnitPrice =  p.UnitPrice,
                TotalAmount =   p.TotalAmount,
                DiscountAmount =  p.DiscountAmount,
                DiscountPercentage =  p.DiscountPercentage,
                CreatedAt =  p.CreatedAt,
                UpdatedAt =  p.UpdatedAt,
            }) : Enumerable.Empty<SaleProductResponse>()
        };
    }
}