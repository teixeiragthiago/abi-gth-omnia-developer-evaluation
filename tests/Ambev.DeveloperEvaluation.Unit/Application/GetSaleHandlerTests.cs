using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new GetSaleHandler(_saleRepository);
    }
    
    [Fact(DisplayName = "Given valid sale command When handling Then returns sale result")]
    public async Task Handle_ValidCommand_GetSaleWithSuccess()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand{ Id = saleId };
        var persistedSale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        persistedSale.Id = saleId;
        
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsForAnyArgs(persistedSale);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(persistedSale.Id);
        result.Should().BeOfType<BaseSaleResult>();
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None);
    }
    
    [Fact(DisplayName = "Given valid sale command When handling Then returns sale result")]
    public async Task Handle_ValidCommand_GetSaleMustThrowExceptionWhenSaleIsNotFound()
    {
        //Arrange
        var command = new GetSaleCommand{ Id = Guid.NewGuid() };
        
        //Act
        Func<Task> sut = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        
        await sut.Should().ThrowAsync<EntityNotFoundException>();

        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None);
    }
}