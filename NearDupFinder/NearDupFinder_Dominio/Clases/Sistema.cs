namespace NearDupFinder_Dominio.Clases;

public class Sistema
{
    private readonly List<Catalogo> _catalogos;
    private readonly List<Usuario> _usuarios = [];

    public Sistema()
    {
        _catalogos = new List<Catalogo>();
        _usuarios.Add(CrearUsuarioAdmin());
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
    
    public Item NormalizarItem(Item item)
    {
        return new Item
        {
            Titulo = Normalizar(item.Titulo),
            Marca = Normalizar(item.Marca),
            Modelo = Normalizar(item.Modelo),
            Categoria = Normalizar(item.Categoria)
        };
    }


    private string Normalizar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        // Convertir a minúsculas
        texto = texto.ToLowerInvariant();

        // Reemplazo de tildes 
        texto = texto.Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ñ", "n")
            .Replace("ü", "u");

        //Reemplaza caracteres especiales por espacios 
        texto = System.Text.RegularExpressions.Regex.Replace(texto, @"[^a-z0-9ñ]", " ");

        return texto;

    }
}