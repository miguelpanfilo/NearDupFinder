using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class CatalogoConfiguraciones : IEntityTypeConfiguration<Catalogo>
{
    public void Configure(EntityTypeBuilder<Catalogo> builder)
    {
        builder.ToTable("Catalogos");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(120)
            .HasColumnType("nvarchar(120)");
        builder.HasIndex(c => c.Titulo).IsUnique();

        builder.Property(c => c.Descripcion)
            .IsRequired(false)
            .HasMaxLength(400)
            .HasColumnType("nvarchar(400)");
    }
}