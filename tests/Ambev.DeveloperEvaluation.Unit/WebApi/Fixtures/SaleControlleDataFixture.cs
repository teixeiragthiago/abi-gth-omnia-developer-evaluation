using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Fixtures;

public static class SaleControlleDataFixture
{
    public static CreateSaleRequest CreateValidSale(List<CreateSaleItemRequest> items)
    {
        return new CreateSaleRequest
        {
            CustomerId =  Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = items
        };
    }

    public static CreateSaleItemRequest CreateValidSaleItem(int quantity, int unitPrice)
    {
        return new CreateSaleItemRequest{ ProductId = Guid.NewGuid(), Quantity = quantity, UnitPrice = unitPrice  };
    }
}