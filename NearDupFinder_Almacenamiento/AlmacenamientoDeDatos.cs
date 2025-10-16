using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Almacenamiento;

public class AlmacenamientoDeDatos
{
    private readonly List<Usuario> _usuarios;
    private readonly List<Catalogo> _catalogos;

    public AlmacenamientoDeDatos()
    {
        _usuarios = [];
        _catalogos = [];
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _usuarios;
    }

    public void AgregarUsuario(Usuario usuario)
    {
        _usuarios.Add(usuario);
    }

    public void RemoverUsuario(Usuario usuario)
    {
        _usuarios.Remove(usuario);
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
}