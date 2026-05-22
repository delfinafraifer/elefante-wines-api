using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using ElefanteWines.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElefanteWines.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _ctx;

    public CategoryRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Categories.AsNoTracking().ToListAsync(ct);

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Categories.FindAsync(new object[] { id }, ct);
}