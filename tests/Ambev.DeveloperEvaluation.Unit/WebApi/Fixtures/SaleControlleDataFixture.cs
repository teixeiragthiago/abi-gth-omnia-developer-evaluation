using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Fixtures;

public static class SaleControlleDataFixture
{
    public static CreateSaleRequest CreateValidSale(List<CreateSaleProductRequest> items)
    {
        return new CreateSaleRequest
        {
            CustomerId =  Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = items
        };
    }

    public static CreateSaleProductRequest CreateValidSaleItem(int quantity, int unitPrice)
    {
        return new CreateSaleProductRequest{ ProductId = Guid.NewGuid(), Quantity = quantity, UnitPrice = unitPrice  };
    }
}