using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _handler = new CancelSaleHandler(_saleRepository, _logger, _mapper);
    }
    
    [Fact(DisplayName = "Given valid sale command When handling Then returns sale result")]
    public async Task Handle_ValidCommand_CancelSaleWithSuccess()
    {
        //Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand{ Id = saleId };
        var cancelledSale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        cancelledSale.Id = saleId;
        
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .ReturnsForAnyArgs(cancelledSale);
        
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(cancelledSale);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cancelledSale.Id);
        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    [Fact(DisplayName = "Given valid sale command When handling Then returns sale result")]
    public async Task Handle_ValidCommand_CancelSaleMustThrowExceptionWhenSaleIsNotFound()
    {
        //Arrange
        var command = new CancelSaleCommand{ Id = Guid.NewGuid() };
        
        //Act
        Func<Task> sut = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        
        await sut.Should().ThrowAsync<EntityNotFoundException>();

        await _saleRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None);
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}