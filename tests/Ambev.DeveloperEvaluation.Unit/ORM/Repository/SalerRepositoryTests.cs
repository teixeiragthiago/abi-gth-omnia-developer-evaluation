using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.ORM.Repository.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repository;

public class SalerRepositoryTests
{
    private readonly DefaultContext _context;
    private readonly ILogger<SaleRepository> _logger;
    private readonly SaleRepository _saleRepository;

    public SalerRepositoryTests()
    {
        _context = CreateInMemoryContext();
        _logger = Substitute.For<ILogger<SaleRepository>>();
        _saleRepository = new SaleRepository(_context, _logger);
    }

    [Fact(DisplayName = "SaleRepository - Insert must persist sale successfully")]
    public async Task Insert_MustPersistWithSuccess()
    {
        // Arrange
        var sale = SaleRepositoryDataFixture.CreateValidSale();

        // Act
        await _saleRepository.InsertAsync(sale, CancellationToken.None);

        // Assert
        var persistedSale = await _context.Sales.FindAsync(sale.Id, CancellationToken.None);

        persistedSale.Should().NotBeNull();
        persistedSale!.Id.Should().Be(sale.Id);
    }
    
    [Fact(DisplayName = "SaleRepository - Insert must throw exception when sale is null")]
    public async Task Insert_WhenSaleIsNull_MustThrowException()
    {
        // Arrange
        Sale sale = null!;

        // Act
        Func<Task> sut = async () =>
            await _saleRepository.InsertAsync(sale,  CancellationToken.None);

        // Assert
        await sut.Should()
            .ThrowAsync<ArgumentNullException>();
    }
    
    [Fact(DisplayName = "SaleRepository - GetByIdAsync must return sale with success")]
    public async Task GetByIdAsync_MustReturnWithSuccess()
    {
        // Arrange
        var sale = SaleRepositoryDataFixture.CreateSaleWithItems();
        
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();

        // Act
        var persistedSale = await _saleRepository.GetByIdAsync(sale.Id, CancellationToken.None);

        // Assert
        persistedSale.Id.Should().Be(sale.Id);
        persistedSale.Should().NotBeNull();
        persistedSale.Items.Should().HaveCount(2);
    }
    
    [Fact(DisplayName = "SaleRepository - UpdateAsync must return sale with success")]
    public async Task UpdateAsync_MustUpdateExistingSale()
    {
        // Arrange
        var sale = SaleRepositoryDataFixture.CreateSaleWithItems();
        sale.AddItem(SaleRepositoryDataFixture.CreateValidSaleItem(400, 1, 0));
        
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
        
        // Act
        var updateResult = await _saleRepository.UpdateAsync(sale, CancellationToken.None);

        // Assert
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(sale.Id);
        updateResult.Items.Should().HaveCount(3);
    }

    
    
    private static DefaultContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new DefaultContext(options);
    }
}