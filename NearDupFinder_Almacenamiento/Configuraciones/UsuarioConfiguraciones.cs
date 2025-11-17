using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento.Configuraciones;

public class UsuarioConfiguraciones: IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        var entidadUsuario = builder;
        entidadUsuario.ToTable("Usuarios");
        entidadUsuario.HasKey(usuario => usuario.Id);
        entidadUsuario.Property(usuario => usuario.Id).ValueGeneratedNever();

        entidadUsuario.OwnsOne(usuario => usuario.Email, email =>
        {
            email.Property(correo => correo.Valor)
                .HasColumnName("Email")
                .IsRequired();
           
            email.HasIndex(correo => correo.Valor).IsUnique();
        });

        entidadUsuario.OwnsOne(usuario => usuario.FechaNacimiento, fechaDeNacimiento =>
        {
            fechaDeNacimiento.Property(fecha => fecha.Anio).HasColumnName("AnioNacimiento");
            fechaDeNacimiento.Property(fecha => fecha.Mes ).HasColumnName("MesNacimiento");
            fechaDeNacimiento.Property(fecha => fecha.Dia ).HasColumnName("DiaNacimiento");
        });

        entidadUsuario.OwnsOne(usuario => usuario.Clave, c =>
        {
            c.Property(clave => clave.Hash).HasColumnName("HashClave");
        });
       
        entidadUsuario.Ignore(usuario => usuario.Roles);

        entidadUsuario.OwnsMany(usuario => usuario.RolesPersistidos, rolesPersistidosDelUsuario =>
        {
            rolesPersistidosDelUsuario.ToTable("UsuarioRoles");
            rolesPersistidosDelUsuario.WithOwner().HasForeignKey("UsuarioId");

            rolesPersistidosDelUsuario.Property(rolPersistido => rolPersistido.Valor)
                .HasColumnName("Rol");
           
            rolesPersistidosDelUsuario.HasKey("UsuarioId", "Valor");
        });
    }
}