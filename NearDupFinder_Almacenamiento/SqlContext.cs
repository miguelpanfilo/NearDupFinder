using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class SqlContext : DbContext
{
   public DbSet<Item> Items { get; set; }
   public DbSet<Usuario> Usuarios { get; set; }
   
   public SqlContext(DbContextOptions<SqlContext> options) : base(options)
   {

       if (Database.IsRelational())
       {
           Database.Migrate();
       }
       else
       {
           Database.EnsureCreated();
       }
   }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       var entidadUsuario = modelBuilder.Entity<Usuario>();
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