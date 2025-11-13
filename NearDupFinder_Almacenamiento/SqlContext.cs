using Microsoft.EntityFrameworkCore;
using NearDupFinder_Almacenamiento.Configuraciones;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class SqlContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Catalogo> Catalogos { get; set; }
    
    public DbSet<ParDuplicado> Duplicados { get; set; }
    public DbSet<Cluster> Clusters { get; set; }
    
    public DbSet<EntradaDeLog> Auditorias { get; set; }
    

    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if (Database.IsRelational())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UsuarioConfiguraciones().Configure(modelBuilder.Entity<Usuario>());
        new ItemConfiguraciones().Configure(modelBuilder.Entity<Item>());
        new CatalogoConfiguraciones().Configure(modelBuilder.Entity<Catalogo>());
        new ParDuplicadosConfiguraciones().Configure(modelBuilder.Entity<ParDuplicado>());
        new ClusterConfiguraciones().Configure(modelBuilder.Entity<Cluster>());
        new AuditoriaConfiguraciones().Configure(modelBuilder.Entity<EntradaDeLog>());
    }
}