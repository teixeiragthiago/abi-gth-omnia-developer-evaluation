using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class IncludeSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<IncludeSaleItemHandler> _logger;
    private readonly IncludeSaleItemHandler _handler;

    public IncludeSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<IncludeSaleItemHandler>>();
        _handler = new IncludeSaleItemHandler(_saleRepository, _mapper, _logger);
    }
    
    [Fact(DisplayName = "Should include new saleItem when sale is valid")]
    public async Task Handle_ValidSale_ShouldIncludeNewItemToSale()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleItemHandlerTestData.CreateValidSaleItemCommand();
        command.SaleId = saleId;
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.Id = saleId;
        
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(sale);
        
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(sale);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        result.Id.Should().Be(saleId);
    }
    
    [Fact(DisplayName = "Should throw EntityNotFoundException when sale is null")]
    public async Task Handle_NotFoundSale_ShouldThrowEntityNotFoundException()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleItemHandlerTestData.CreateValidSaleItemCommand();
        command.SaleId = saleId;
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.Id = saleId;

        await _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        //Act
        Func<Task> sut = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await sut.Should().ThrowAsync<EntityNotFoundException>();
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact(DisplayName = "Should throw DomainException when sale is cancelled")]
    public async Task Handle_CancelledSale_ShouldThrowDomainExceptionWhenSaleIsCancelled()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleItemHandlerTestData.CreateValidSaleItemCommand();
        command.SaleId = saleId;
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.Id = saleId;
        sale.Cancel();

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(sale);
        
        //Act
        Func<Task> sut = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await sut.Should().ThrowAsync<DomainException>();
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}