using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken ct = default);
    Task<Sale> InsertAsync(Sale sale, CancellationToken ct = default);
}