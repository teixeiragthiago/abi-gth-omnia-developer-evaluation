using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<SaleRepository> _logger;

    public SaleRepository(DefaultContext context, ILogger<SaleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Sale> InsertAsync(Sale sale, CancellationToken ct = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            _logger.LogInformation("Creating new sale");
            
            await _context.Sales.AddAsync(sale, ct);
            await _context.SaveChangesAsync(ct);
            
            await transaction.CommitAsync(ct);

            _logger.LogInformation("Sale created successfully {SaleId}", sale.Id);
            
            return sale;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating sale");
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Sales
            .Include(items => items)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Cancelling sale {SaleId}", sale.Id);
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(ct);
        
            _logger.LogInformation("Sale cancelled successfully {SaleId}", sale.Id);
            
            return sale;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error cancelling sale");
            throw;
        }
    }
}