namespace NearDupFinder_Dominio.Clases;

public class Sistema
{
    private readonly List<Catalogo> _catalogos;
    private readonly List<Usuario> _usuarios = new();
    private readonly List<Item> _items = new();

    public Sistema()
    {
        _catalogos = new List<Catalogo>();
        _usuarios.Add(CrearUsuarioAdmin());
    }

    public event Action? ItemsChanged;

    public void AgregarItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Add(item);

        // Notifica que cambió la lista
        ItemsChanged?.Invoke();
    }

    public List<Item> ObtenerItems()
    {
        // Devuelve una copia para que afuera no modifiquen la lista interna
        return new List<Item>(_items);
    }
    /* Comienzo espacio Catalogo*/
    public void AgregarCatalogo(Catalogo c)
    {
        if (c == null)
        {
            throw new ArgumentNullException(nameof(c), "El catálogo no puede ser null");
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
    /* Fin espacio Catalogo*/
    
    /* Comienzo espacio Usuario*/
    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }

    public Usuario? AutenticarUsuario(string? email, string? clave)
    {
        Email emailAValidar;
        try
        {
            emailAValidar = Email.Crear(email);
        }
        catch
        {
            return null;
        }

        foreach (var usuario in _usuarios)
        {
            if (usuario.Email.Igual(emailAValidar) && usuario.VerificarContrasena(clave))
                return usuario;
        }
        return null;
    }
    /*Fin espacio usuario*/
    
    public void ProbarItemsChanged()
    {
        ItemsChanged?.Invoke();
    }
    public string ObtenerMensaje()
    {
        return "¡El sistema está conectado correctamente!";
    }

}