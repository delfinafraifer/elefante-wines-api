using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using ElefanteWines.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElefanteWines.Infrastructure.Repositories;

public class WineRepository : IWineRepository
{
    private readonly ApplicationDbContext _ctx;

    public WineRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<Wine?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Wines.FindAsync(new object[] { id }, ct);

    // AsNoTracking() = lectura pura, no se trackean cambios. Más rápido y menos memoria.
    public async Task<IEnumerable<Wine>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Wines.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<Wine>> SearchByNameAsync(string term, CancellationToken ct = default)
        => await _ctx.Wines
                     .AsNoTracking()
                     .Where(w => w.Name.Contains(term))
                     .ToListAsync(ct);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Wines.AnyAsync(w => w.Id == id, ct);

    public async Task AddAsync(Wine wine, CancellationToken ct = default)
    {
        await _ctx.Wines.AddAsync(wine, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Wine wine, CancellationToken ct = default)
    {
        _ctx.Wines.Update(wine);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var wine = await _ctx.Wines.FindAsync(new object[] { id }, ct);
        if (wine is not null)
        {
            _ctx.Wines.Remove(wine);
            await _ctx.SaveChangesAsync(ct);
        }
    }
}