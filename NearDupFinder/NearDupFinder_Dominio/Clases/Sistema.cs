namespace NearDupFinder_Dominio.Clases;

public class Sistema
{
    private readonly List<Catalogo> _catalogos;

    public Sistema()
    {
        _catalogos = new List<Catalogo>();
    }

    public void AgregarCatalogo(Catalogo c)
    {
        if (c == null)
        {
            throw new ArgumentException("El catálogo no puede ser null");
        }
        else if (_catalogos.Contains(c))
        {
            throw new InvalidOperationException("Ya existe un catálogo con ese título");
        }

    _catalogos.Add(c);
    }
    
    public Catalogo? ObtenerCatalogoPorTitulo(string titulo)
        => _catalogos.FirstOrDefault(c => c.Titulo == titulo);

    public int CantidadDeCatalogos()
    {
        return _catalogos.Count;
    }
}