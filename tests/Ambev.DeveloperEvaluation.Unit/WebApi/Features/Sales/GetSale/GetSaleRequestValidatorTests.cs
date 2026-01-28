using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales.CreateSale.GetSale;

public class GetSaleRequestValidatorTests
{
    private readonly GetSaleRequestValidator _validator;

    public GetSaleRequestValidatorTests()
    {
        _validator = new GetSaleRequestValidator();
    }
    
    [Fact(DisplayName = "Should have validation error when Id is empty")]
    public void Id_Empty_Should_Have_ValidationError()
    {
        //Arrange
        var request = new GetSaleRequest(Guid.Empty);
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact(DisplayName = "Should not have validation error when Id is valid")]
    public void Id_Valid_ShouldNot_Have_ValidationError()
    {
        //Arrange
        var request = new GetSaleRequest(Guid.NewGuid());
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}