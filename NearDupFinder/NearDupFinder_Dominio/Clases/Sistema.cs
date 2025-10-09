using System.Text.RegularExpressions;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Struct;
using NearDupFinder_Dominio.Controladores;

namespace NearDupFinder_Dominio.Clases;

public enum TipoDuplicado
{
    τ_alert,
    τ_dup
}

public record struct ParDuplicado 
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
    private readonly List<Usuario> _usuarios;
    private readonly List<int> _idsItemsGlobal;
    private readonly LectorCsv _lectorCsv;
    public List<ParDuplicado > DuplicadosGlobales { get; set; }
    private readonly GestorUsuarios _gestorUsuarios;


    public Sistema()
    {
        _usuarios = new List<Usuario>();
        _gestorUsuarios = new GestorUsuarios(this);
        _usuarios.Add(_gestorUsuarios.CrearUsuarioAdmin());
        _catalogos = new List<Catalogo>();
        DuplicadosGlobales = new List<ParDuplicado >();
        _idsItemsGlobal = new List<int>();
        _lectorCsv = new LectorCsv(this);
    }

    //------------------------------------------------------------------------//
    /* Comienzo espacio Usuarios*/
    
    public bool AltaUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        return _gestorUsuarios.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
    }
    
    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        return _gestorUsuarios.EditarDatosDelUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
    }
    
    public bool EliminarUsuario(string email)
    {
        return _gestorUsuarios.BorrarUsuario(email);
    }

    public IReadOnlyList<Usuario> ObtenerUsuarios()
    {
        return _usuarios.AsReadOnly(); 
    }

    internal void AgregarUsuarioDeLaLista(Usuario usuario)
    {
        _usuarios.Add(usuario);
    }

    internal void RemoverUsuarioDeLaLista(Usuario usuario)
    {
        _usuarios.Remove(usuario);
    }
    
    public Usuario? BuscarUsuarioPorId(int id)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Id == id)
                return usuario;
        return null;    
    }
    
    public Usuario? BuscarUsuarioPorEmail(Email email)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Email.Igual(email))
                return usuario;
        return null;
    }
    
    public bool ModificarClave(string email,string claveActual, string? claveNueva)
    {
        Usuario? usuarioValidado = ValidarUsuario(email, claveActual);
        if( usuarioValidado is not null)
            return _gestorUsuarios.ModificarClave(usuarioValidado, claveNueva);
        return false;
    }
    
    public Usuario? ValidarUsuario(string? email, string? clave)
    {
        return _gestorUsuarios.AutenticarUsuario(email, clave);
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
        catalogoHogar.AgregarItem(new Item
        {
            Titulo = "Aspiradoraaaaaa",
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
        Item nuevoItem1 = new Item
        {
            Titulo = "Pelota de fútbol",
            Descripcion = "Pelota oficial tamaño 5",
            Categoria = "Fútbol",
            Marca = "Adidas",
            Modelo = "Al Rihla"
        };
        catalogoDeportes.AgregarItem(nuevoItem1);
        Item nuevoItem = new Item
        {
            Titulo = "Pelota de fútbollll",
            Descripcion = "Pelota oficial tamaño 5",
            Categoria = "Fútbol",
        };
        catalogoDeportes.AgregarItem(nuevoItem);

        // Agregar catálogos al sistema
        _catalogos.Add(catalogoTecno);
        _catalogos.Add(catalogoHogar);
        _catalogos.Add(catalogoDeportes);
        
        catalogoDeportes.ConfirmarClusters(nuevoItem1,nuevoItem);
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
public void AltaItemConAltaDuplicados(string catalogoTitulo, Item nuevoItem)
   {
        var catalogo = ObtenerCatalogoPorTitulo(catalogoTitulo);

        ValidarCatalogoYItem(catalogo, nuevoItem);

        AsegurarIdUnico(nuevoItem);
        catalogo.AgregarItem(nuevoItem);
    
        var duplicadosDelItem = DetectarDuplicados(nuevoItem, catalogo);
        AgregarDuplicadosADuplicadosGlobales(duplicadosDelItem);
    }

    


public void ActualizarItemEnCatalogo(Catalogo catalogo, ItemEditDataTransfer dto)
{
    var itemAEditar = catalogo.Items.FirstOrDefault(i => i.Id == dto.Id);
    if (itemAEditar == null)
        throw new ItemException("No se encontró el item a actualizar.");

    itemAEditar.EditarTitulo(dto.Titulo);
    itemAEditar.EditarDescripcion(dto.Descripcion);
    itemAEditar.EditarCategoria(dto.Categoria);
    itemAEditar.EditarMarca(dto.Marca);
    itemAEditar.EditarModelo(dto.Modelo);
}

public void EliminarItem(string catalogo, ItemEditDataTransfer dto)
{
    var catalogoBuscado = ObtenerCatalogoPorTitulo(catalogo);
    if (catalogoBuscado == null)
        throw new ArgumentException("El catálogo no existe.");

    var item = catalogoBuscado.Items.FirstOrDefault(i => i.Id == dto.Id);
    if (item == null)
        throw new ItemException("El item no existe en el catálogo.");

    ValidarCatalogoYItem(catalogoBuscado, item);

    catalogoBuscado.EliminarItem(item);

    EliminarDuplicadosPrevios(item);
    ActualizarEstadoDuplicadosEnCatalogo(catalogoBuscado);
}




private void ValidarCatalogoYItem(Catalogo catalogo, Item item)
{
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

    ActualizarEstadoDuplicadosEnCatalogo(catalogo);
}

private void AgregarDuplicadosADuplicadosGlobales(IEnumerable<ParDuplicado>? duplicados)
{
    if (duplicados == null) return;

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


private void ActualizarEstadoDuplicadosEnCatalogo(Catalogo catalogo)
{
    foreach (var item in catalogo.Items) 
    {
        bool tieneDuplicados = DuplicadosGlobales.Any(d => d.ItemA.Id == item.Id || d.ItemB.Id == item.Id);
        item.EstadoDuplicado = tieneDuplicados;
    }
}

    private void AsegurarIdUnico(Item item)
    {
        int idApropiado = item.Id;
        while (_idsItemsGlobal.Contains(idApropiado))
            idApropiado++;
        item.ModificarId(idApropiado);
        _idsItemsGlobal.Add(idApropiado);
    }
    
    public bool IdExisteEnListaDeIdGlobal(int id)
    {
        return _idsItemsGlobal.Contains(id);
    }

    public int CantidadDeItemsGlobal()
    {
        return _idsItemsGlobal.Count;
    }

    public void DescartarParDuplicado(ParDuplicado duplicadoADescartar)
    {
        DuplicadosGlobales.Remove(duplicadoADescartar);

        duplicadoADescartar.ItemA.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemA.Id == duplicadoADescartar.ItemA.Id || unDuplicado.ItemB.Id == duplicadoADescartar.ItemA.Id);
        duplicadoADescartar.ItemB.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemA.Id == duplicadoADescartar.ItemB.Id || unDuplicado.ItemB.Id == duplicadoADescartar.ItemB.Id);
    }

  
//------------------------------------------------------------------------
// Fin espacio Item

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

    public List<ParDuplicado > DetectarDuplicados(Item itemA, Catalogo catalogo)
    {
        List<ParDuplicado > listaDuplicados = new List<ParDuplicado >();

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

                ParDuplicado  duplicado = new ParDuplicado 
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

//--------------------------------------------------------------
/* Inicio Lectura de CSV */

    public void ImportarItemsDesdeCsv(List<string> titulos, int cantidad, List<Fila> filas)
    {
        _lectorCsv.LeerCsv(titulos, cantidad, filas);
        _lectorCsv.ImportarItems();
        _lectorCsv.Limpiar();
    }

//--------------------------------------------------------------
/* Fin de Lectura de CSV */

}