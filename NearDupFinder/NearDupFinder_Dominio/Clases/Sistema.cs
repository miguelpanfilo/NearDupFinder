using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Struct;

namespace NearDupFinder_Dominio.Clases;

public enum TipoDuplicado
{
    τ_alert,
    τ_dup
}

public struct Duplicados
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

    public Sistema()
    {
        _catalogos = new List<Catalogo>();
        _usuarios.Add(CrearUsuarioAdmin());
        PrecargarCatalogos();

    }


    private void PrecargarCatalogos()
    {
        
        var catalogoTecno = new Catalogo("Tecnología");
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


    //------------------------------------------------------------------------
    /* Comienzo espacio Catalogo*/
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

    public IReadOnlyCollection<Catalogo> Catalogos => _catalogos;

    public Catalogo? ObtenerCatalogoPorTitulo(string titulo)
        => _catalogos.FirstOrDefault(c => c.Titulo == titulo);

    public int CantidadDeCatalogos()
    {
        return _catalogos.Count;
    }
    /* Fin espacio Catalogo*/

    // Inicio funciones de interfaz 
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
   

//Fin de funciones de interfaz 

    /* Comienzo espacio Usuario*/
    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997, 12, 27);
        Usuario adminUsuario = Usuario.Crear("admin", "admin", email, fecha);
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
    //------------------------------------------------------------------------

    //------------------------------------------------------------------------
    // Inicio de deteccion de duplicados 
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
//Fin de deteccion de duplicados 


}