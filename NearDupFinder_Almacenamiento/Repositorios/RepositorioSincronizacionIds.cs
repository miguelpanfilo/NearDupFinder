using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioSincronizacionIds : IRepositorioSincronizacionIds
{
    private readonly SqlContext _context;

    public RepositorioSincronizacionIds(SqlContext context)
    {
        _context = context;
    }

    private const int listaVacia = 0;
    public int ObtenerMaximoIdItems()
        => _context.Items.Any() ? _context.Items.Max(i => i.Id) : listaVacia;

    public int ObtenerMaximoIdCatalogos()
        => _context.Catalogos.Any() ? _context.Catalogos.Max(c => c.Id) : listaVacia;

    public int ObtenerMaximoIdCluster()
        => _context.Clusters.Any() ? _context.Clusters.Max(c => c.Id) : listaVacia;
}