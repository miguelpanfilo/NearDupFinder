using NearDupFinder_Almacenamiento;
using NearDupFinder_Interfaces;

public class RepositorioSincronizacionIds : IRepositorioSincronizacionIds
{
    private readonly SqlContext _context;

    public RepositorioSincronizacionIds(SqlContext context)
    {
        _context = context;
    }

    public int ObtenerMaximoIdItems()
        => _context.Items.Any() ? _context.Items.Max(i => i.Id) : 0;

    public int ObtenerMaximoIdCatalogos()
        => _context.Catalogos.Any() ? _context.Catalogos.Max(c => c.Id) : 0;
}