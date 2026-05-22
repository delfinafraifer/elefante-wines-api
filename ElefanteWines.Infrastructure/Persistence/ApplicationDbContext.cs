using ElefanteWines.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElefanteWines.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Un DbSet por tabla — refleja el modelo relacional
    public DbSet<Wine> Wines { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Busca automáticamente todas las clases IEntityTypeConfiguration<T>
        // en este assembly y las aplica. Así no tenemos que llamarlas una por una.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}