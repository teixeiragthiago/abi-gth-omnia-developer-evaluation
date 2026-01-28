using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Policies;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class IncludeSaleProductHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<IncludeSaleProductHandler> _logger;
    private readonly IncludeSaleProductHandler _handler;
    private readonly IDiscountPolicy _discountPolicy;
    private readonly IMediator _mediator;

    public IncludeSaleProductHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _logger = Substitute.For<ILogger<IncludeSaleProductHandler>>();
        _discountPolicy = Substitute.For<IDiscountPolicy>();
        _mediator = Substitute.For<IMediator>();
        _handler = new IncludeSaleProductHandler(_saleRepository, _logger, _discountPolicy, _mediator);
    }
    
    [Fact(DisplayName = "Should include new saleItem when sale is valid")]
    public async Task Handle_ValidSale_ShouldIncludeNewItemToSale()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleProductHandlerTestData.CreateValidSaleItemCommand();
        command.SaleId = saleId;
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.Id = saleId;
        
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(sale);
        
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(sale);

        _mediator.Publish(Arg.Any<SaleProductIncludedNotification>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _mediator.Publish(Arg.Any<SaleProductIncludedNotification>(), Arg.Any<CancellationToken>());
        result.Id.Should().Be(saleId);
    }
    
    [Fact(DisplayName = "Should throw EntityNotFoundException when sale is null")]
    public async Task Handle_NotFoundSale_ShouldThrowEntityNotFoundException()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleProductHandlerTestData.CreateValidSaleItemCommand();
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
        await _mediator.DidNotReceive().Publish(Arg.Any<SaleCancelledNotification>(), Arg.Any<CancellationToken>());
    }
    
    [Fact(DisplayName = "Should throw DomainException when sale is cancelled")]
    public async Task Handle_CancelledSale_ShouldThrowDomainExceptionWhenSaleIsCancelled()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = IncludeSaleProductHandlerTestData.CreateValidSaleItemCommand();
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
        await _mediator.DidNotReceive().Publish(Arg.Any<SaleCancelledNotification>(), Arg.Any<CancellationToken>());
    }
}