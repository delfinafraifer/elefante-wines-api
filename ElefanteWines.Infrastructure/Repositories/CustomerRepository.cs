using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using ElefanteWines.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElefanteWines.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _ctx;

    public CustomerRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Customers.FindAsync(new object[] { id }, ct);

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _ctx.Customers
                     .AsNoTracking()
                     .FirstOrDefaultAsync(c => c.Email == email, ct);

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Customers.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        await _ctx.Customers.AddAsync(customer, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}