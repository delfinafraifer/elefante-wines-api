using ElefanteWines.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElefanteWines.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    // GUIDs FIJOS — nunca usar Guid.NewGuid() en HasData()
    public static readonly Guid TintosId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000001");
    public static readonly Guid BlancosId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000002");
    public static readonly Guid EspumantesId =
        new Guid("a1b2c3d4-0000-0000-0000-000000000003");

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Datos iniciales (seed) — GUIDs hardcodeados obligatoriamente
        builder.HasData(
            new Category { Id = TintosId,      Name = "Tintos" },
            new Category { Id = BlancosId,     Name = "Blancos" },
            new Category { Id = EspumantesId,  Name = "Espumantes" }
        );
    }
}