using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;
using Ambev.DeveloperEvaluation.Unit.WebApi.Fixtures;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.IncludeSaleItem;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales;

public class SalesControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOptions<SaleProductOptions> _options;
    private readonly SalesController _saleController;

    public SalesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _options = Options.Create(new SaleProductOptions
        {
            UnitQuantity = new UnitQuantity { Min = 1, Max = 20 },
            UnitPrice = new UnitPrice { Min = 0, Max = 99999 }
        });
        _saleController = new SalesController(_mediator, _mapper, _options);
    }
    
    [Fact(DisplayName = "Sales Controller: Create Sale Must Return With Success")]
    public async Task CreateSale_MustReturnWithSuccessWhenRequestIsValid()
    {
        //Arrange
        var saleItem1 = SaleControlleDataFixture.CreateValidSaleItem(1, 20);
        var saleItem2 = SaleControlleDataFixture.CreateValidSaleItem(2, 30);
        
        var saleRequest = SaleControlleDataFixture.CreateValidSale(items: new List<CreateSaleProductRequest>() { saleItem1, saleItem2 });
        var command = _mapper.Map<CreateSaleCommand>(saleRequest);

        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.CreateSale(saleRequest, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();

        var acceptedResult = result as CreatedResult;
        acceptedResult!.StatusCode.Should().Be(StatusCodes.Status201Created);    
    }
    
    [Fact(DisplayName = "Sales Controller: Create Sale Must Return BadRequest")]
    public async Task CreateSale_MustReturnBadRequestWhenRequestIsInvalid()
    {
        //Arrange
        var saleRequest = SaleControlleDataFixture.CreateValidSale(null);
        var command = _mapper.Map<CreateSaleCommand>(saleRequest); 
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.CreateSale(saleRequest, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);    
    }
    
    [Fact(DisplayName = "Sales Controller: Get Sale Must Return With Success")]
    public async Task GetSale_MustReturnWithSuccessWhenRequestIsValid()
    {
        //Arrange
        var command = new GetSaleCommand{Id = Guid.NewGuid()};
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.GetSale(Guid.NewGuid(), CancellationToken.None);

        //Assert

        var acceptedResult = result as OkObjectResult;
        acceptedResult!.StatusCode.Should().Be(StatusCodes.Status200OK);    
    }
    
    [Fact(DisplayName = "Sales Controller: Get Sale must return bad request when request is invalid")]
    public async Task GetSale_MustReturnBadRequestWhenRequestIsInvalid()
    {
        //Arrange
        var command = new GetSaleCommand{Id = Guid.NewGuid()};
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.GetSale(Guid.Empty, CancellationToken.None);

        //Assert

        var acceptedResult = result as BadRequestObjectResult;
        acceptedResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);    
    }
    
    [Fact(DisplayName = "Sales Controller: Cancel Sale Must Return With NoContent")]
    public async Task CancelSale_MustReturnNoContentWhenRequestIsValid()
    {
        //Arrange
        var command = new CancelSaleCommand{Id = Guid.NewGuid()};
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.CancelSale(Guid.NewGuid(), CancellationToken.None);

        //Assert

        var acceptedResult = result as NoContentResult;
        acceptedResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);    
    }
    
    [Fact(DisplayName = "Sales Controller: Cancel Sale must return bad request when request is invalid")]
    public async Task CancelSale_MustReturnBadRequestWhenRequestIsInvalid()
    {
        //Arrange
        var command = new CancelSaleCommand{Id = Guid.NewGuid()};
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.CancelSale(Guid.Empty, CancellationToken.None);

        //Assert

        var badRquestResult = result as BadRequestObjectResult;
        badRquestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);    
    }
    
    [Fact(DisplayName = "Sales Controller: Include sale item Must Return With Success")]
    public async Task IncludeSaleItem_MustReturnSucessWhenRequestIsValid()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        
        var request = new IncludeSaleProductRequest
        {
            ProductId = Guid.NewGuid(),
            UnitPrice = 10m,
            Quantity = 2
        };
            
        var command = new IncludeSaleProductCommand
        {
            ProductId = request.ProductId,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity
        };
        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.IncludeSaleItem(saleId, request, CancellationToken.None);

        //Assert

        var okStatusResult = result as OkResult;
        okStatusResult!.StatusCode.Should().Be(StatusCodes.Status200OK);    
    }
    
    [Fact(DisplayName = "Sales Controller: Include sale item must return bad request when request is invalid")]
    public async Task IncludeSaleItem_MustReturnBadRequestWhenRequestIsInvalid()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        
        var request = new IncludeSaleProductRequest
        {
            ProductId = Guid.Empty,
            UnitPrice = 0,
            Quantity = 0
        };
            
        var command = new IncludeSaleProductCommand
        {
            ProductId = request.ProductId,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity
        };        
        await _mediator.Send(command, CancellationToken.None);
            
        //Act
        var result = await _saleController.IncludeSaleItem(saleId, request, CancellationToken.None);

        //Assert

        var badRequstResult = result as BadRequestObjectResult;
        badRequstResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);    
    }
}