using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public class IncludeSaleItemHandlerTestData
{
    public static IncludeSaleItemCommand CreateValidSaleItemCommand()
    {
        return new IncludeSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            UnitPrice = 10,
            Quantity = 5,
        };
    }
}