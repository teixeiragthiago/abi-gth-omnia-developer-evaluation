using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales.CreateSale.CancelSale;

public class CancelSaleRequestValidatorTests
{
    private readonly CancelSaleRequestValidator _validator;
    
    public CancelSaleRequestValidatorTests()
    {
        _validator = new CancelSaleRequestValidator();
    }
    
    [Fact(DisplayName = "Should have validation error when Id is empty")]
    public void Id_Empty_Should_Have_ValidationError()
    {
        //Arrange
        var request = new CancelSaleRequest(Guid.Empty);
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact(DisplayName = "Should not have validation error when Id is valid")]
    public void Id_Valid_ShouldNot_Have_ValidationError()
    {
        //Arrange
        var request = new CancelSaleRequest(Guid.NewGuid());
    
        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}