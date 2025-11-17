using NearDupFinder_Dominio.Clases;
using Microsoft.EntityFrameworkCore;
using NearDupFinder_Interfaces;                 

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioCatalogos : RepositorioGenerico<Catalogo>, IRepositorioCatalogos
    {
        public RepositorioCatalogos(SqlContext context) : base(context)
        {
        }
    
        public Catalogo? ObtenerPorTitulo(string titulo)
        {
            return _context.Set<Catalogo>()
                .Include(c => c.Items)
                .Include(c => c.Clusters)
                .ThenInclude(cl => cl.PertenecientesCluster)
                .FirstOrDefault(c => c.Titulo == titulo);
        }

        public new Catalogo? ObtenerPorId(int id)
        {
            return _context.Set<Catalogo>()
                .Include(c => c.Items)
                .Include(c => c.Clusters)
                .ThenInclude(cl => cl.PertenecientesCluster)
                .FirstOrDefault(c => c.Id == id);
        }
        
        public Catalogo? ObtenerParaEliminacionPorId(int idCatalogo)
        {
            return _context.Set<Catalogo>()
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == idCatalogo);
        }
        
        public void LimpiarSeguimiento()
        {
            _context.ChangeTracker.Clear();
        }
    }
}