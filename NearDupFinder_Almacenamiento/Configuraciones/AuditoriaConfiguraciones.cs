using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class AuditoriaConfiguraciones : IEntityTypeConfiguration <EntradaDeLog>
{
    public void Configure(EntityTypeBuilder<EntradaDeLog> builder)
    {
        const int maximoLargoDetallesAuditorias = 300;
        const int maximoLargoAccionAuditorias = 100;
        const int maximoLargoDeUnEmail = 320;
        builder.ToTable("Auditorias");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd(); 

        builder.Property(a => a.Timestamp)
            .IsRequired();

        builder.Property(a => a.Usuario)
            .IsRequired()
            .HasMaxLength(maximoLargoDeUnEmail);

        builder.Property(a => a.Accion)
            .IsRequired()
            .HasConversion<string>() 
            .HasMaxLength(maximoLargoAccionAuditorias);

        builder.Property(a => a.Detalles)
            .HasMaxLength(maximoLargoDetallesAuditorias);
    }
}