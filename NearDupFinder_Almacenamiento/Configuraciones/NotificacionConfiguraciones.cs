using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class NotificacionConfiguraciones : IEntityTypeConfiguration<Notificacion>
{
    public void Configure(EntityTypeBuilder<Notificacion> builder)
    {
        builder.Property(n => n.EmailUsuario)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Mensaje)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(n => n.Leida)
            .IsRequired();
    }
}