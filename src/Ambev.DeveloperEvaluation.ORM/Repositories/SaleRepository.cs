using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository :  ISaleRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<SaleRepository> _logger;

    public SaleRepository(DefaultContext context, ILogger<SaleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<Sale> CreateAsync(Sale sale, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task CancelAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Sale> UpdateAsync(Sale sale, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}