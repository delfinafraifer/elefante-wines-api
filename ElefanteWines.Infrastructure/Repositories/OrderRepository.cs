using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using ElefanteWines.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElefanteWines.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _ctx;

    public OrderRepository(ApplicationDbContext ctx) => _ctx = ctx;

    // Eager loading: traemos la orden + sus items en una sola query (JOIN)
    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Orders
                     .Include(o => o.Items)
                     .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => await _ctx.Orders
                     .AsNoTracking()
                     .Where(o => o.CustomerId == customerId)
                     .OrderByDescending(o => o.CreatedAt)
                     .ToListAsync(ct);

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Orders
                     .AsNoTracking()
                     .OrderByDescending(o => o.CreatedAt)
                     .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _ctx.Orders.AddAsync(order, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}