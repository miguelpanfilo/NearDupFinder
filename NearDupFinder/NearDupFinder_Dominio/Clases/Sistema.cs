using System.Security.Claims;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Controladores;

namespace NearDupFinder_Dominio.Clases;

public class Sistema
{
    private readonly List<Catalogo> _catalogos;
    private readonly List<Usuario> _usuarios;
    private readonly List<int> _idsItemsGlobal;
    private readonly LectorCsv _lectorCsv;
    public List<ParDuplicado > DuplicadosGlobales { get; set; }
    private readonly GestorUsuarios _gestorUsuarios;
    private readonly GestorDuplicados _gestorDuplicados;
    private readonly List<LogEntry> _auditoria = new List<LogEntry>();
    private string _usuarioActual = "No hay usuario logueado";    

    public void SetUsuarioActual(string email)
    {
        _usuarioActual = email;
    }



    public Sistema()
    {
        _usuarios = new List<Usuario>();
        _gestorUsuarios = new GestorUsuarios(this);
        _usuarios.Add(_gestorUsuarios.CrearUsuarioAdmin());
        _catalogos = new List<Catalogo>();
        DuplicadosGlobales = new List<ParDuplicado >();
        _idsItemsGlobal = new List<int>();
        _lectorCsv = new LectorCsv(this);
        _gestorDuplicados = new GestorDuplicados();

    }

  
    
    private readonly Dictionary<AccionLog, string> _descripcionesAccion = new()
    {
        { AccionLog.AltaUsuario, "Creacion de usuario" },
        { AccionLog.EditarUsuario, "Modificacion de usuario" },
        { AccionLog.AltaItem, "Alta de item" },
        { AccionLog.EliminarItem, "Eliminación de item" },
        { AccionLog.DeteccionDuplicados, "Detección duplicados automatica" },
        { AccionLog.ConfirmarDuplicado ,"Confirmación duplicado"},
        { AccionLog.FusionarDuplicado,"Fusión Cluster" },
        { AccionLog.DescartarDuplicado,"Descartar duplicado"},
        {AccionLog.EditarItem,"Editar item"},
        
        
    };
    public void RegistrarLog(AccionLog accion, string detalles)
    {

        var entry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Usuario = _usuarioActual,
            Accion = accion,
            Detalles = $"{_descripcionesAccion[accion]}: {detalles}"
        };

        _auditoria.Add(entry);
    }
    public IReadOnlyList<LogEntry> ObtenerLogs() => _auditoria.AsReadOnly();

    //------------------------------------------------------------------------//
    /* Comienzo espacio Usuarios*/
    

 public bool AltaUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaAltaDeUsuario = _gestorUsuarios.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaAltaDeUsuario)
            RegistrarLog(AccionLog.AltaUsuario, $"Usuario: '{email}'");
        return pasaAltaDeUsuario;
    }
    
    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaModificarUsuario = _gestorUsuarios.EditarDatosDelUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaModificarUsuario)
            RegistrarLog(AccionLog.EditarUsuario, $"Usuario modificado: '{email}'");
        return pasaModificarUsuario;    }
    
    public bool EliminarUsuario(string email)
    {
        bool pasaEliminarUsuario = _gestorUsuarios.BorrarUsuario(email);
        if (pasaEliminarUsuario)
            RegistrarLog(AccionLog.EliminarItem, $"Usuario eliminado: '{email}'");
        return pasaEliminarUsuario;    }

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
        
        RegistrarLog(AccionLog.AltaItem, $"Item agregado: '{nuevoItem.Titulo}' en catálogo '{catalogoTitulo}'");

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
    
    RegistrarLog(AccionLog.EditarItem, $"Item actualizado: '{dto.Titulo}' en catálogo '{catalogo.Titulo}'");

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
    RegistrarLog(AccionLog.EliminarItem, $"Item eliminado: '{item.Titulo}' del catálogo '{catalogoBuscado.Titulo}'");

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

public void ConfirmarDuplicadoConLog(ParDuplicado duplicado)
{
    

    RegistrarLog(AccionLog.ConfirmarDuplicado, $"Se confirmó duplicado: Item '{duplicado.ItemA.Titulo}' y '{duplicado.ItemB.Titulo}'");
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
        RegistrarLog(AccionLog.DescartarDuplicado, $"Par duplicado descartado: '{duplicadoADescartar.ItemA.Titulo}' + '{duplicadoADescartar.ItemB.Titulo}'");

    }

  
//------------------------------------------------------------------------
// Fin espacio Item

//------------------------------------------------------------------------
/* Inicio de deteccion de duplicados */
    public List<ParDuplicado> DetectarDuplicados(Item itemA, Catalogo catalogo)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var duplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        stopwatch.Stop();

        RegistrarLog(
            AccionLog.DeteccionDuplicados,
            $"Detección de duplicados para item '{itemA.Titulo}' en catálogo '{catalogo.Titulo}' completada en {stopwatch.ElapsedMilliseconds} ms."
        );

        return duplicados;
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