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
    
    public Usuario? BuscarUsuarioPorEmail(Email email)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Email.Igual(email))
                return usuario;
        return null;
    }
    public Usuario? BuscarUsuarioPorId(int id)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Id == id)
                return usuario;
        return null;    
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