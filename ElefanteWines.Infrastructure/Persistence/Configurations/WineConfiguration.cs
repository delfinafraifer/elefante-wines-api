using ElefanteWines.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElefanteWines.Infrastructure.Persistence.Configurations;

public class WineConfiguration : IEntityTypeConfiguration<Wine>
{
    public void Configure(EntityTypeBuilder<Wine> builder)
    {
        builder.ToTable("Wines");
        builder.HasKey(w => w.Id);

        // El Guid lo genera el dominio, no la BD
        builder.Property(w => w.Id).ValueGeneratedNever();

        builder.Property(w => w.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(w => w.Description)
               .HasMaxLength(2000);

        builder.Property(w => w.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");  // precisión monetaria

        builder.Property(w => w.Stock)
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(w => w.Varietal)
               .HasMaxLength(100);

        builder.Property(w => w.Year);

        builder.Property(w => w.CategoryId).IsRequired();
        builder.Property(w => w.CreatedAt).IsRequired();

        // Índice para acelerar búsquedas por nombre
        builder.HasIndex(w => w.Name);
    }
}