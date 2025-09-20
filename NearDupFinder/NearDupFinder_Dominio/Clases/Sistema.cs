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
        _catalogos.Add(c);
    }

    public int CantidadDeCatalogos()
    {
        return _catalogos.Count;
    }
}