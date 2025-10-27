using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class SqlContext : DbContext
{
   public DbSet<Item> Items { get; set; }
   
   public SqlContext(DbContextOptions<SqlContext> options) : base(options){

       this.Database.Migrate(); //Ejecutara las migracione al crear la BD

   }
}