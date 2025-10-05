using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Struct;

namespace NearDupFinder_Dominio.Clases;

public enum TipoDuplicado
{
    τ_alert,
    τ_dup
}

public record struct Duplicados
{
    public Item ItemA { get; set; }
    public Item ItemB { get; set; }
    public float Score { get; set; }
    public TipoDuplicado Tipo { get; set; }
    public float JaccardTitulo { get; set; }
    public float JaccardDescripcion { get; set; }
    public int ScoreMarca { get; set; }
    public int ScoreModelo { get; set; }
    public string [] TokensCompartidosTitulo { get; set; }
    public string [] TokensCompartidosDescripcion { get; set; }
}

public class Sistema
{
    private const string TokenPattern = @"\W+";

    private readonly List<Catalogo> _catalogos;
    private readonly List<Usuario> _usuarios = [];
    public List<Duplicados> DuplicadosGlobales { get; set; }

    public Sistema()
    {
        _catalogos = new List<Catalogo>();
        _usuarios.Add(CrearUsuarioAdmin());
        PrecargarCatalogos();
        DuplicadosGlobales = new List<Duplicados>();
    }

    //------------------------------------------------------------------------//
    /* Comienzo espacio Usuarios*/
    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }
    
    public bool CrearUsuario(string? nombre, string? apellido, string? email, int anio, int mes, int dia, string? clave, List<Rol>? roles)
    {
        Email correo;
        Fecha fecha;
        Usuario nuevoUsuario;
        Contrasena contrasena;
        try
        {
            correo = Email.Crear(email);
            if (BuscarUsuarioPorEmail(correo) is not null)
                return false;

            fecha = Fecha.Crear(anio,mes,dia);
            contrasena = Contrasena.Crear(clave);
            nuevoUsuario = Usuario.Crear(nombre,apellido,correo,fecha);
        }
        catch
        {
            return false;
        }
        
        nuevoUsuario.CambiarContrasena(contrasena);
        if(roles is null)
            return false;
        foreach (var rol in roles)
            nuevoUsuario.AgregarRol(rol);
        _usuarios.Add(nuevoUsuario);
        
        return true;
    }
    
    public List<Usuario> ObtenerUsuarios()
    {
        return _usuarios;
    }

    public Usuario? BuscarUsuarioPorId(int id)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Id == id)
                return usuario;
        return null;    
    }
    
    private Usuario? BuscarUsuarioPorEmail(Email email)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Email.Igual(email))
                return usuario;
        return null;
    }

    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol>? roles)
    {
        Email correo;
        Fecha fecha;
        Usuario? usuarioAModificar;
        // La contraseña es opcional, en caso que no se ingrese no se cambia.
        Contrasena? contrasena = null;
        try
        {
            correo = Email.Crear(email);
            usuarioAModificar = BuscarUsuarioPorEmail(correo);
            if(usuarioAModificar is null)
                return false;
            
            fecha = Fecha.Crear(anio,mes,dia);
            if(!string.IsNullOrWhiteSpace(clave))
                contrasena = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || roles is null)
            return false;
        
        usuarioAModificar.Nombre = nombre;
        usuarioAModificar.Apellido = apellido;
        usuarioAModificar.FechaNacimiento = fecha;
        usuarioAModificar.ObtenerRoles().Except(roles).ToList().ForEach(r => usuarioAModificar.RemoverRol(r));
        roles.Except(usuarioAModificar.ObtenerRoles()).ToList().ForEach(r => usuarioAModificar.AgregarRol(r));
        
        if(!string.IsNullOrWhiteSpace(clave))
            usuarioAModificar.CambiarContrasena(contrasena);
        
        return true;
    }
    
    public bool ModificarClave(string? email, string? clave)
    {
        Email correo;
        Contrasena contrasena;
        try
        {
            correo = Email.Crear(email);
            contrasena = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuarioPorEmail(correo);
        if (usuario is null)
            return false;

        return usuario.CambiarContrasena(contrasena);
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
        Usuario? usuario = BuscarUsuarioPorEmail(emailAValidar);
        if (usuario is null)
            return null;
        
        if (usuario.VerificarContrasena(clave))
            return usuario;
        
        return null;
    }

    public bool EliminarUsuario(string? email)
    {
        Email emailUsuario;
        try
        {
            emailUsuario = Email.Crear(email);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuarioPorEmail(emailUsuario);
        if (usuario is null)
            return false;
        
        _usuarios.Remove(usuario);
        
        return true;
    }
    //------------------------------------------------------------------------
    /* Fin espacio Usuario */

    //------------------------------------------------------------------------
    /* Comienzo espacio Catalogo*/
    
    private void PrecargarCatalogos()
    {
        var catalogoTecno = new Catalogo("Tecnología");
        catalogoTecno.CambiarDescripcion("Componentes eletronicos");
        catalogoTecno.AgregarItem(new Item
        {
            Titulo = "Laptop HP",
            Descripcion = "Laptop 15 pulgadas",
            Categoria = "Computadoras",
            Marca = "HP",
            Modelo = "Pavilion"
        });
        catalogoTecno.AgregarItem(new Item
        {
            Titulo = "Teléfono Samsung",
            Descripcion = "Galaxy S24",
            Categoria = "Celulares",
            Marca = "Samsung",
            Modelo = "S24"
        });

        var catalogoHogar = new Catalogo("Hogar");
        catalogoHogar.CambiarDescripcion("Electrodomesticos de Hogar");
        catalogoHogar.AgregarItem(new Item
        {
            Titulo = "Silla de comedor",
            Descripcion = "Silla de madera maciza",
            Categoria = "Muebles",
            Marca = "Ikea",
            Modelo = "Nordic"
        });
        catalogoHogar.AgregarItem(new Item
        {
            Titulo = "Aspiradora",
            Descripcion = "Aspiradora sin bolsa 1200W",
            Categoria = "Electrodomésticos",
            Marca = "Philips",
            Modelo = "PowerPro"
        });

        var catalogoDeportes = new Catalogo("Deportes");
        catalogoDeportes.CambiarDescripcion("Actividades deportivas, y equipo para hacer deporte");
        catalogoDeportes.AgregarItem(new Item
        {
            Titulo = "Bicicleta",
            Descripcion = "Bicicleta de montaña 21 cambios",
            Categoria = "Ciclismo",
            Marca = "Trek",
            Modelo = "X-Caliber"
        });
        catalogoDeportes.AgregarItem(new Item
        {
            Titulo = "Pelota de fútbol",
            Descripcion = "Pelota oficial tamaño 5",
            Categoria = "Fútbol",
            Marca = "Adidas",
            Modelo = "Al Rihla"
        });

        // Agregar catálogos al sistema
        _catalogos.Add(catalogoTecno);
        _catalogos.Add(catalogoHogar);
        _catalogos.Add(catalogoDeportes);
    }
    public void AgregarCatalogo(Catalogo catalogo)
    {
        if (catalogo is null)
        {
            throw new ArgumentNullException(nameof(catalogo), "El parametro no puede ser null");
        }

        if (_catalogos.Contains(catalogo))
        {
            throw new InvalidOperationException("Ya existe un catálogo con ese título");
        }

        _catalogos.Add(catalogo);
    }

    public void EliminarCatalogo(Catalogo catalogo)
    {
        if (catalogo is null)
            throw new ArgumentNullException(nameof(catalogo), "El parametro no puede ser null");
        if (!_catalogos.Contains(catalogo))
            throw new InvalidOperationException("No existe un catálogo con ese título");
        _catalogos.Remove(catalogo);
    }

    public void CambiarTituloCatalogo(Catalogo catalogo, string titulo)
    {
        var candidatoCata = ObtenerCatalogoPorTitulo(titulo);
        if (candidatoCata != null && !ReferenceEquals(candidatoCata, catalogo))
        {
            throw new InvalidOperationException("El Título del catálogo ya existe");
        }

        catalogo.CambiarTitulo(titulo);
    }
    public IReadOnlyCollection<Catalogo> Catalogos => _catalogos.AsReadOnly();

    public Catalogo? ObtenerCatalogoPorTitulo(string titulo)
        => _catalogos.FirstOrDefault(c=> c.Titulo.Equals(titulo ?? "", StringComparison.OrdinalIgnoreCase));

    public int CantidadDeCatalogos()
    {
        return _catalogos.Count;
    }
    //------------------------------------------------------------------------
    /* Fin espacio Catalogo*/
    
    //------------------------------------------------------------------------
    // Inicio espacio Item 
    public void ActualizarItemEnCatalogo(Catalogo catalogo, ItemEditDataTransfer dto)
    {
        var original = catalogo.Items.FirstOrDefault(i => i.Id == dto.Id);
        if (original == null)
            throw new ItemException("No se encontró el item a actualizar.");

        original.Titulo = dto.Titulo;
        original.Descripcion = dto.Descripcion;
        original.Categoria = dto.Categoria;
        original.Marca = dto.Marca;
        original.Modelo = dto.Modelo;
    }
    public void AltaItemConAltaDuplicados(string catalogoTitulo, Item nuevoItem)
    {
        var catalogo = ObtenerCatalogoPorTitulo(catalogoTitulo);

        ValidarCatalogoYItem(catalogo, nuevoItem);

        catalogo.AgregarItem(nuevoItem);
        
        var duplicadosDelItem = DetectarDuplicados(nuevoItem, catalogo);
    
        AgregarDuplicadosADuplicadosGlobales(duplicadosDelItem);

    }
    private void ValidarCatalogoYItem(Catalogo catalogo, Item item)
    {
        if (catalogo == null)
            throw new ItemException("Debe seleccionar un catálogo válido.");
        if (item == null || string.IsNullOrWhiteSpace(item.Titulo) || string.IsNullOrWhiteSpace(item.Descripcion))
            throw new ItemException("Título y Descripción son obligatorios.");
    }

    
    public void ActualizarDuplicadosPara(Catalogo catalogo, Item itemEditado)
    {
        if (catalogo == null || itemEditado == null)
            throw new ArgumentNullException();

       EliminarDuplicadosPrevios(itemEditado);
       
       var nuevosDuplicados = DetectarDuplicados(itemEditado, catalogo);
       
       AgregarDuplicadosADuplicadosGlobales(nuevosDuplicados);
       
       


    }
    private void AgregarDuplicadosADuplicadosGlobales(IEnumerable<Duplicados>? duplicados)
    {
      
        foreach (var dup in duplicados)
        {
            DuplicadosGlobales.Add(dup);

            dup.ItemA.EstadoDuplicado = true;
            dup.ItemB.EstadoDuplicado = true;
        }
    }

    
    private void EliminarDuplicadosPrevios(Item item)
    {
        var duplicadosABorrar = DuplicadosGlobales
            .Where(d => d.ItemA.Id == item.Id || d.ItemB.Id == item.Id)
            .ToList();

        foreach (var duplicado in duplicadosABorrar)
            DuplicadosGlobales.Remove(duplicado);
    }




//------------------------------------------------------------------------
/* Fin espacio Items */

//------------------------------------------------------------------------
/* Inicio de deteccion de duplicados */
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
            .Where(t => t.Length > 1)
            .ToArray();
    }

    public Item NormalizarItem(Item item)
    {
        // Normalizamos cada propiedad del item
        string tituloNormalizado = Normalizar(item.Titulo);
        string descripcionNormalizada = Normalizar(item.Descripcion);

        // Lanzar excepción si título o descripción quedan vacíos
        if (string.IsNullOrEmpty(tituloNormalizado) || string.IsNullOrEmpty(descripcionNormalizada))
        {
            throw new InvalidOperationException("El título y la descripción no pueden quedar vacío tras normalizar.");
        }

        string marcaNormalizada = Normalizar(item.Marca);
        string modeloNormalizada = Normalizar(item.Modelo);
        string categoriaNormalizada = Normalizar(item.Categoria);

        // Retornar un nuevo item con los valores normalizados
        return new Item
        {
            Titulo = tituloNormalizado,
            Descripcion = descripcionNormalizada,
            Marca = marcaNormalizada,
            Modelo = modeloNormalizada,
            Categoria = categoriaNormalizada
        };
    }

    public string Normalizar(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return string.Empty;

        texto = texto.ToLowerInvariant();

        texto = texto.Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ñ", "n")
            .Replace("ü", "u");

        texto = System.Text.RegularExpressions.Regex.Replace(texto, @"[^a-z0-9]", " ");
        texto = System.Text.RegularExpressions.Regex.Replace(texto, @"\s+", " ").Trim();
        return texto;
    }

    public int CalcularNumTokensUnion(string[] tokens1, string[] tokens2)
    {
        ArgumentNullException.ThrowIfNull(tokens1);
        ArgumentNullException.ThrowIfNull(tokens2);

        return tokens1.Union(tokens2).Count();
    }

    public int CalcularNumTokensInterseccion(string[] tokens1, string[] tokens2)
    {
        ArgumentNullException.ThrowIfNull(tokens1);
        ArgumentNullException.ThrowIfNull(tokens2);

        return tokens1.Intersect(tokens2).Count();
    }

    public float CalcularJaccard(string[] tokens1, string[] tokens2)
    {
        ArgumentNullException.ThrowIfNull(tokens1);
        ArgumentNullException.ThrowIfNull(tokens2);

        float numTokensUnion = CalcularNumTokensUnion(tokens1, tokens2);
        if (numTokensUnion == 0)
            return 0;

        float numTokensInterseccion = CalcularNumTokensInterseccion(tokens1, tokens2);
        float valorJaccard = numTokensInterseccion / numTokensUnion;

        return valorJaccard;
    }

    public float CalcularScore(float jaccardTitulo, float jaccardDescripcion, float marcaEq, float modeloEq)
    {
        if (jaccardTitulo < 0f || jaccardTitulo > 1f || jaccardDescripcion < 0f || jaccardDescripcion > 1f)
            throw new ArgumentOutOfRangeException();
        if ((marcaEq != 0f && marcaEq != 1f) || (modeloEq != 0f && modeloEq != 1f))
            throw new ArgumentOutOfRangeException();

        float score = 0.45f * jaccardTitulo + 0.35f * jaccardDescripcion + 0.10f * marcaEq + 0.10f * modeloEq;

        return score;
    }

    private static int IgualdadBinaria(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            return 0;

        return string.Equals(a, b, StringComparison.Ordinal) ? 1 : 0;
    }

    public List<Duplicados> DetectarDuplicados(Item itemA, Catalogo catalogo)
    {
        List<Duplicados> listaDuplicados = new List<Duplicados>();

        Item itemNormalizadoA = NormalizarItem(itemA);
        ItemTokenizado itemTokenizadoA = TokenizarItem(itemNormalizadoA);

        foreach (Item itemB in catalogo.Items)
        {
            if (itemB.Id == itemA.Id)
                continue;

            Item itemNormalizadoB = NormalizarItem(itemB);
            ItemTokenizado itemTokenizadoB = TokenizarItem(itemNormalizadoB);

            float jaccardTitulo = CalcularJaccard(itemTokenizadoA.TokenTitulo, itemTokenizadoB.TokenTitulo);
            float jaccardDescripcion =
                CalcularJaccard(itemTokenizadoA.TokenDescripcion, itemTokenizadoB.TokenDescripcion);

            int scoreMarca = IgualdadBinaria(itemNormalizadoA.Marca, itemNormalizadoB.Marca);
            int scoreModelo = IgualdadBinaria(itemNormalizadoA.Modelo, itemNormalizadoB.Modelo);

            float score = CalcularScore(jaccardTitulo, jaccardDescripcion, scoreMarca, scoreModelo);

            if (score >= 0.60f)
            {
                string[] tokensCompartidosTitulo =
                    itemTokenizadoA.TokenTitulo.Intersect(itemTokenizadoB.TokenTitulo).ToArray();
                string[] tokensCompartidosDescripcion = itemTokenizadoA.TokenDescripcion
                    .Intersect(itemTokenizadoB.TokenDescripcion).ToArray();

                Duplicados duplicado = new Duplicados
                {
                    ItemA = itemA,
                    ItemB = itemB,
                    Score = score,
                    Tipo = TipoDuplicado.τ_alert,
                    JaccardTitulo = jaccardTitulo,
                    JaccardDescripcion = jaccardDescripcion,
                    ScoreMarca = scoreMarca,
                    ScoreModelo = scoreModelo,
                    TokensCompartidosTitulo = tokensCompartidosTitulo,
                    TokensCompartidosDescripcion = tokensCompartidosDescripcion
                };

                if (score >= 0.75f)
                    duplicado.Tipo = TipoDuplicado.τ_dup;

                listaDuplicados.Add(duplicado);
            }
        }

        listaDuplicados.Sort((x, y) =>
        {
            int c = y.Score.CompareTo(x.Score);
            if (c != 0) return c;
            return x.ItemB.Id.CompareTo(y.ItemB.Id);
        });

        return listaDuplicados;
    }
//--------------------------------------------------------------
/* Fin de deteccion de duplicados */

}