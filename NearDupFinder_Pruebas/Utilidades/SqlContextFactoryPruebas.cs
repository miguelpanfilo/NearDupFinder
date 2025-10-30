using Microsoft.EntityFrameworkCore;
using NearDupFinder_Almacenamiento;

namespace NearDupFinder_Pruebas.Utilidades
{
    public static class SqlContextFactoryPruebas
    {
        public static DbContextOptions<SqlContext> CrearOpcionesInMemory(string nombreBaseDeDatos)
        {
            DbContextOptions<SqlContext> opciones = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(nombreBaseDeDatos)
                .EnableSensitiveDataLogging()
                .Options;

            return opciones;
        }
        
        public static SqlContext CrearContexto(DbContextOptions<SqlContext> opciones)
        {
            SqlContext contexto = new SqlContext(opciones);
            return contexto;
        }
        
        public static void LimpiarBaseDeDatos(SqlContext contexto)
        {
            contexto.Database.EnsureDeleted();
            contexto.Database.EnsureCreated();
        }
    }
}