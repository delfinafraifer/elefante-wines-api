using ElefanteWines.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElefanteWines.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.Property(o => o.CustomerId).IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired();

        builder.Property(o => o.Total)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        // Guardar el enum como string legible ("Pending", "Confirmed", ...),
        // no como número (0, 1, 2). Mucho más claro al inspeccionar la BD.
        builder.Property(o => o.Status)
               .HasConversion<string>()
               .HasMaxLength(20);

        // Relación 1:N con OrderItems — si se borra la orden, se borran los items
        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}