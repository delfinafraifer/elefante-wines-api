using ElefanteWines.Domain.Entities;

namespace ElefanteWines.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
}