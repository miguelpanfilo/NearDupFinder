using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class AlmacenamientoDeDatos
{
    private readonly List<Catalogo> _catalogos;

    public AlmacenamientoDeDatos()
    {
        _catalogos = [];
    }

    public List<Catalogo> ObtenerCatalogos()
    {
        return _catalogos;
    }

    public void AgregarCatalogo(Catalogo catalogo)
    {
        _catalogos.Add(catalogo);
    }

    public void RemoverCatalogo(Catalogo catalogo)
    {
        _catalogos.Remove(catalogo);
    }
    

    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
    {
        return _catalogos.FirstOrDefault(c=> c.Titulo.Equals(titulo ?? "", StringComparison.OrdinalIgnoreCase));
    }

    public Catalogo? ObtenerCatalogoPorId(int id)
    {
        return ObtenerCatalogos().FirstOrDefault(c => c.Id == id);
    }
}