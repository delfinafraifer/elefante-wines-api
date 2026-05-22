using ElefanteWines.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElefanteWines.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.UnitPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.WineId).IsRequired();
        builder.Property(i => i.OrderId).IsRequired();

        // Subtotal es una propiedad calculada en C# — NO se persiste en la BD
        builder.Ignore(i => i.Subtotal);
    }
}