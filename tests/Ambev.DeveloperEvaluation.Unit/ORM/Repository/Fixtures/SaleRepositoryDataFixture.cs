using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repository.Fixtures;

[ExcludeFromCodeCoverage]
public static class SaleRepositoryDataFixture
{
    public static Sale CreateValidSale()
        => new Sale(Guid.NewGuid(), Guid.NewGuid());

    public static SaleProduct CreateValidSaleItem(decimal unitPrice, int quantity, decimal discount)
        => new SaleProduct(Guid.NewGuid(), unitPrice, quantity, discount);

    public static Sale CreateSaleWithItems()
    {
        var sale = CreateValidSale();
        var item1 = CreateValidSaleItem(100, 2, 5);
        var item2 = CreateValidSaleItem(10, 5, 10);
        
        sale.AddItem(item1);
        sale.AddItem(item2);
        
        return sale;
    }

}