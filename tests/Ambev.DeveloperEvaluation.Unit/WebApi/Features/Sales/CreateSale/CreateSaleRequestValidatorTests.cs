using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidatorTests
{
    private readonly CreateSaleRequestValidator _validator;
    private readonly SaleProductOptions _options;

    public CreateSaleRequestValidatorTests()
    {
        _options = new SaleProductOptions
        {
            UnitQuantity = new UnitQuantity { Min = 1, Max = 20 },
            UnitPrice = new UnitPrice { Min = 0, Max = 99999 }
        };
        _validator = new CreateSaleRequestValidator(_options);
    }
    
    [Fact(DisplayName = "Should have validation error when BranchId is empty")]
    public void BranchId_Empty_Should_Have_ValidationError()
    {
        //Arrange
        var request = new CreateSaleRequest();
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.BranchId);
    }
    
    [Fact(DisplayName = "Should have validation error when CustomerId is empty")]
    public void CustomerId_Empty_Should_Have_ValidationError()
    {
        //Arrange
        var request = new CreateSaleRequest();
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }
    
    [Fact(DisplayName = "Should have validation error when Items is empty")]
    public void Items_Empty_Should_Have_ValidationError()
    {
        //Arrange
        var request = new CreateSaleRequest();
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }
    
    [Fact(DisplayName = "Should have not validation error when validation do not pass")]
    public void Valid_Request_Should_Pass()
    {
        //Arrange
        var request = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductRequest>()
            {
                new CreateSaleProductRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 0.01m
                },
                new CreateSaleProductRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 4,
                    UnitPrice = 0.02m
                },
            }
        };
        
        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact(DisplayName = "Should have validation error when validation do not pass")]
    public void Invalid_Request_ShouldNot_Pass()
    {
        //Arrange
        var request = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Products = new List<CreateSaleProductRequest>()
            {
                new CreateSaleProductRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 20,
                    UnitPrice = 10m
                },
                new CreateSaleProductRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 22,
                    UnitPrice = 0.02m
                },
            }
        };
        
        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }
}