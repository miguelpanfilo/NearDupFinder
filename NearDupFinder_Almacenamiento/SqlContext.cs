using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class SqlContext : DbContext
{
   public DbSet<Item> Items { get; set; }
   
   public DbSet<Usuario> Usuarios { get; set; }
   public DbSet<EntradaDeLog> Auditorias { get; set; }

   
   public SqlContext(DbContextOptions<SqlContext> options) : base(options){

       this.Database.Migrate(); //Ejecutara las migracione al crear la BD

   }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       const int maximoLargoDeUnEmail = 320;
       const int maximoLargoDetallesAuditorias = 300;
       const int maximoLargoAccionAuditorias = 100;
       var entidadUsuario = modelBuilder.Entity<Usuario>();
       entidadUsuario.ToTable("Usuarios");
       entidadUsuario.HasKey(usuario => usuario.Id);
       entidadUsuario.Property(usuario => usuario.Id).ValueGeneratedNever();


       entidadUsuario.Property(usuario => usuario.Nombre).IsRequired().HasMaxLength(100);
       entidadUsuario.Property(usuario => usuario.Apellido).IsRequired().HasMaxLength(100);

       entidadUsuario.OwnsOne(usuario => usuario.Email, email =>
       {
           email.Property(correo => correo.Valor)
               .HasColumnName("Email")
               .IsRequired()
               .HasMaxLength(maximoLargoDeUnEmail);

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
           c.Property(clave => clave.Hash).HasColumnName("HashClave").IsRequired();
       });
       
       entidadUsuario.Ignore(usuario => usuario.Roles);

       entidadUsuario.OwnsMany(usuario => usuario.RolesPersistidos, rolesPersistidosDelUsuario =>
       {
           rolesPersistidosDelUsuario.ToTable("UsuarioRoles");
           rolesPersistidosDelUsuario.WithOwner().HasForeignKey("UsuarioId");

           rolesPersistidosDelUsuario.Property(rolPersistido => rolPersistido.Valor)
               .HasColumnName("Rol")
               .HasMaxLength(64)
               .IsRequired();

           rolesPersistidosDelUsuario.HasKey("UsuarioId", "Valor");
       });
       
       var entidadCatalogo = modelBuilder.Entity<Catalogo>();
       entidadCatalogo.ToTable("Catalogos");
       entidadCatalogo.HasKey(c => c.Id);
       entidadCatalogo.Property(c => c.Id).ValueGeneratedOnAdd();

       entidadCatalogo.Property(c => c.Titulo)
           .IsRequired()
           .HasMaxLength(120)
           .HasColumnType("nvarchar(120)");
       entidadCatalogo.HasIndex(c => c.Titulo).IsUnique();

       entidadCatalogo.Property(c => c.Descripcion)
           .IsRequired(false)
           .HasMaxLength(400)
           .HasColumnType("nvarchar(400)");

       modelBuilder.Entity<Item>(e =>
       {
           e.ToTable("Items");
           e.HasKey(i => i.Id);
           e.Property(i => i.Id).ValueGeneratedOnAdd();

           e.HasOne<Catalogo>()
               .WithMany(nameof(Catalogo.Items))
               .HasForeignKey("CatalogoId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

           e.HasIndex("CatalogoId");
        
           e.HasOne<Cluster>()
               .WithMany(nameof(Cluster.PertenecientesCluster))
               .HasForeignKey("ClusterId")
               .OnDelete(DeleteBehavior.ClientSetNull);
       });

       modelBuilder.Entity<Cluster>(e =>
       {
           e.ToTable("Clusters");
           e.HasKey(cl => cl.Id);
           e.Property(cl => cl.Id).ValueGeneratedOnAdd();

           e.HasOne<Catalogo>()
               .WithMany(nameof(Catalogo.Clusters))
               .HasForeignKey("CatalogoId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

           e.HasIndex("CatalogoId");

           e.Metadata.FindNavigation(nameof(Cluster.PertenecientesCluster))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

           e.HasOne(c => c.Canonico)
               .WithMany()
               .HasForeignKey("CanonicoId")
               .OnDelete(DeleteBehavior.ClientSetNull);
       });
       modelBuilder.Entity<EntradaDeLog>(e =>
       {
           e.ToTable("Auditorias");
           e.HasKey(a => a.Id);
           e.Property(a => a.Id)
               .ValueGeneratedOnAdd(); 

           e.Property(a => a.Timestamp)
               .IsRequired();

           e.Property(a => a.Usuario)
               .IsRequired()
               .HasMaxLength(maximoLargoDeUnEmail);

           e.Property(a => a.Accion)
               .IsRequired()
               .HasConversion<string>() 
               .HasMaxLength(maximoLargoAccionAuditorias);

           e.Property(a => a.Detalles)
               .HasMaxLength(maximoLargoDetallesAuditorias);
       });
       
       
       
       
   }
}   