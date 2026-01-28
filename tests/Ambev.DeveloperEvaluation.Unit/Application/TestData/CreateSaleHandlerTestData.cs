using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CreateSaleHandlerTestData
{
    public static CreateSaleCommand GenerateValidCommand()
    {
        return new CreateSaleCommand
        {
            BranchId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Products = new[]
            {
                new SaleProductDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 10m
                }
            }
        };
    }

    public static CreateSaleCommand GenerateInvalidCommand()
    {
        return new CreateSaleCommand
        {
            BranchId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Products = new[]
            {
                new SaleProductDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 0,
                    UnitPrice = 0
                }
            }
        };
    }
}


