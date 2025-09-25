using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Struct;

namespace NearDupFinder_Dominio.Clases;


public class Sistema
{
    private const string TokenPattern = @"\W+";
    
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
    
    public ItemTokenizado TokenizarItem(Item item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        
        return new ItemTokenizado
        {
            TokenTitulo = Tokenizar(item.Titulo),
            TokenDescripcion = Tokenizar(item.Descripcion)
        };
    }

    private static string[] Tokenizar(string texto)
    {
        return Regex.Split(texto, TokenPattern)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();
    }

    public string Normalizar(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return string.Empty;

        return texto; 
    }

}