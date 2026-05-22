using ElefanteWines.Domain.Entities;

namespace ElefanteWines.Application.Interfaces;

public interface IWineRepository
{
    Task<Wine?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Wine>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Wine>> SearchByNameAsync(string term, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Wine wine, CancellationToken ct = default);
    Task UpdateAsync(Wine wine, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}