using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public class IncludeSaleProductHandlerTestData
{
    public static IncludeSaleProductCommand CreateValidSaleItemCommand()
    {
        return new IncludeSaleProductCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            UnitPrice = 10,
            Quantity = 5,
        };
    }
}