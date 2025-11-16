using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class CatalogoConfiguraciones : IEntityTypeConfiguration<Catalogo>
{
    public void Configure(EntityTypeBuilder<Catalogo> builder)
    {
        const int maximoCaracteresTitulo = 120;
        const int maximoCaracteresDescripcion = 400;
        builder.ToTable("Catalogos");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(maximoCaracteresTitulo)
            .HasColumnType("nvarchar(120)");
        builder.HasIndex(c => c.Titulo).IsUnique();

        builder.Property(c => c.Descripcion)
            .IsRequired(false)
            .HasMaxLength(maximoCaracteresDescripcion)
            .HasColumnType("nvarchar(400)");
    }
}