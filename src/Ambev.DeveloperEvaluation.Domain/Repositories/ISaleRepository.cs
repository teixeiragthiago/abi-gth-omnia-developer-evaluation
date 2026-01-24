using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken ct = default);
    Task CancelAsync(Guid id, CancellationToken ct = default);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken ct = default);
}