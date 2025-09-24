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
    
  
  
  
  
  //DETECCION DE DUPLICADOS
  
  //Normalizacion
    public Item NormalizarItem(Item item)
    {
       
        var titulo = Normalizar(item.Titulo);
        var descripcion = Normalizar(item.Descripcion);

        // Lanzar excepción solo si título o descripción quedan vacíos
        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descripcion))
            throw new InvalidOperationException("El título y la descripción no puede quedar vacío tras normalizar.");

        // Normalizar propiedades no obligatorias
        var marca = Normalizar(item.Marca);
        var modelo = Normalizar(item.Modelo);
        var categoria = Normalizar(item.Categoria);

        return new Item
        {
            Titulo = titulo,
            Descripcion = descripcion,
            Marca = marca,
            Modelo = modelo,
            Categoria = categoria
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

        // Reemplaza caracteres especiales por espacios 
        texto = System.Text.RegularExpressions.Regex.Replace(texto, @"[^a-z0-9]", " ");

        // Colapsa múltiples espacios y recorta
        texto = System.Text.RegularExpressions.Regex.Replace(texto, @"\s+", " ").Trim();

        

        return texto;
    }

    
    
    
    // Tokenizacion
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

    public int CalcularNumTokensUnion(string[]? tokens1, string[]? tokens2)
    {
        if (tokens1 is null || tokens2 is null)
            return -1;

        return tokens1.Union(tokens2).Count();
    }

    public int CalcularNumTokensInterseccion(string[] tokens1, string[] tokens2)
    {
        IEnumerable<string> interseccion = tokens1.Intersect(tokens2);
        int numTokensInterseccion = interseccion.Count();
        
        return numTokensInterseccion;
    }
}