using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class UsuarioConfiguraciones: IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        const int maximoLargoDeUnEmail = 320;
        builder.ToTable("Usuarios");
        builder.HasKey(usuario => usuario.Id);
        builder.Property(usuario => usuario.Id).ValueGeneratedNever();


        builder.Property(usuario => usuario.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(usuario => usuario.Apellido).IsRequired().HasMaxLength(100);

        builder.OwnsOne(usuario => usuario.Email, email =>
        {
            email.Property(correo => correo.Valor)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(maximoLargoDeUnEmail);

            email.HasIndex(correo => correo.Valor).IsUnique();
        });

        builder.OwnsOne(usuario => usuario.FechaNacimiento, fechaDeNacimiento =>
        {
            fechaDeNacimiento.Property(fecha => fecha.Anio).HasColumnName("AnioNacimiento");
            fechaDeNacimiento.Property(fecha => fecha.Mes ).HasColumnName("MesNacimiento");
            fechaDeNacimiento.Property(fecha => fecha.Dia ).HasColumnName("DiaNacimiento");
        });

        builder.OwnsOne(usuario => usuario.Clave, c =>
        {
            c.Property(clave => clave.Hash).HasColumnName("HashClave").IsRequired();
        });
       
        builder.Ignore(usuario => usuario.Roles);

        builder.OwnsMany(usuario => usuario.RolesPersistidos, rolesPersistidosDelUsuario =>
        {
            rolesPersistidosDelUsuario.ToTable("UsuarioRoles");
            rolesPersistidosDelUsuario.WithOwner().HasForeignKey("UsuarioId");

            rolesPersistidosDelUsuario.Property(rolPersistido => rolPersistido.Valor)
                .HasColumnName("Rol")
                .HasMaxLength(64)
                .IsRequired();

            rolesPersistidosDelUsuario.HasKey("UsuarioId", "Valor");
        });

    }
}