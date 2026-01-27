using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IOptions<SaleUnitOptions> _options;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _options = Options.Create(new SaleUnitOptions
        {
            UnitQuantity = new UnitQuantity { Min = 1, Max = 20 },
            UnitPrice = new UnitPrice { Min = 0, Max = 99999 }
        });
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _options, _logger);
    }
    
    [Fact(DisplayName = "Given valid sale command When handling Then returns sale result")]
    public async Task Handle_ValidCommand_ReturnsSaleResult()
    {
        //Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        var saleEntity = new Sale(command.CustomerId, command.BranchId)
        {
            Id = Guid.NewGuid()
        };

        var saleResult = new BaseSaleResult
        {
            Id = saleEntity.Id
        };

        _saleRepository.InsertAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleEntity);

        _mapper.Map<BaseSaleResult>(saleEntity).Returns(saleResult);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(saleEntity.Id);

        await _saleRepository.Received(1).InsertAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<BaseSaleResult>(saleEntity);
    }
    
    [Fact(DisplayName = "Given invalid sale command When handling Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        //Arrange
        var command = CreateSaleHandlerTestData.GenerateInvalidCommand();

        //Act
        Func<Task> sut = async () => await _handler.Handle(command, CancellationToken.None);

        //Assert
        await sut.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().InsertAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}