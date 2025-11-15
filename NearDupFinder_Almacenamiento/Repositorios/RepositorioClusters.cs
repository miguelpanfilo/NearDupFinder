using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioClusters: RepositorioGenerico<Cluster>, IRepositorioClusters
{
    private readonly DbSet<Cluster> _clusters;
    public RepositorioClusters(SqlContext context) : base(context)
    {
        _clusters = context.Set<Cluster>();
    }
    
    public void LimpiarCanonicoPorCatalogo(int idCatalogo)
    {
        var clusters = _context.Set<Cluster>()
            .Where(c => EF.Property<int>(c, "CatalogoId") == idCatalogo
                        && c.Canonico != null) 
            .ToList();

        foreach (var cluster in clusters)
        {
            cluster.Canonico = null;
        }

        _context.SaveChanges();
    }
}