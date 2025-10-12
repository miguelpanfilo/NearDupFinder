using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Controladores;

namespace NearDupFinder_Dominio.Clases;

public class Sistema
{
    private readonly List<Usuario> _usuarios;
    private readonly List<Catalogo> _catalogos;
    private readonly List<int> _idsItemsGlobal;
    public readonly List<ParDuplicado> DuplicadosGlobales;
    private readonly GestorUsuarios _gestorUsuarios;
    private readonly GestorDuplicados _gestorDuplicados;
    private readonly LectorCsv _lectorCsv;
    private readonly List<LogEntry> _auditoria = new List<LogEntry>();
    private string _usuarioActual = "No hay usuario logueado"; 
   
    public void SetUsuarioActual(string email)
    {
        _usuarioActual = email;
    }
    public void LogoutUsuario()
    {
        _usuarioActual = "No hay usuario logueado";
    }
    private readonly Dictionary<LogEntry.AccionLog, string> _descripcionesAccion = new()
    {
        { LogEntry.AccionLog.AltaUsuario, "Creacion de usuario" },
        { LogEntry.AccionLog.EditarUsuario, "Modificacion de usuario" },
        { LogEntry.AccionLog.AltaItem, "Alta de item" },
        { LogEntry.AccionLog.EliminarItem, "Eliminación de item" },
        { LogEntry.AccionLog.DeteccionDuplicados, "Detección duplicados automatica" },
        { LogEntry.AccionLog.ConfirmarDuplicado ,"Confirmación de duplicado"},
        { LogEntry.AccionLog.FusionarCluster,"Fusión de cluster" },
        { LogEntry.AccionLog.DescartarDuplicado,"Descartar duplicado"},
        {LogEntry.AccionLog.EditarItem,"Editar item"},
        {LogEntry.AccionLog.EliminarUser,"Eliminacion de usuario"},
        
        
    };
    public void RegistrarLog(LogEntry.AccionLog accion, string detalles)
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
        PrecargarCatalogos();
    }

    //------------------------------------------------------------------------//
    /* Comienzo espacio Usuarios*/
    
    
    public bool AltaUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaAltaDeUsuario = _gestorUsuarios.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaAltaDeUsuario)
            RegistrarLog(LogEntry.AccionLog.AltaUsuario, $"Usuario: '{email}'");
        return pasaAltaDeUsuario;
    }
    
    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaModificarUsuario = _gestorUsuarios.EditarDatosDelUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaModificarUsuario)
            RegistrarLog(LogEntry.AccionLog.EditarUsuario, $"Usuario modificado: '{email}'");
        return pasaModificarUsuario;    }


    
    public bool EliminarUsuario(string email)
    {
        bool pasaEliminarUsuario = _gestorUsuarios.BorrarUsuario(email);
        if (pasaEliminarUsuario)
            RegistrarLog(LogEntry.AccionLog.EliminarUser, $"Usuario eliminado: '{email}'");
        return pasaEliminarUsuario;    }

    public IReadOnlyList<Usuario> ObtenerUsuarios()
    {
        return _usuarios.AsReadOnly(); 
    }
    
    public IReadOnlyCollection<Rol>ObtenerRolesDeUsuario(Usuario usuario)
    {
        return usuario.ObtenerRoles(); 
    }
    
    public bool UsuarioTieneRol(Usuario usuario, Rol rol)
    {
        return usuario.TieneRol(rol); 
    }

    public void AgregarUsuarioALaLista(Usuario usuario)
    {
        _usuarios.Add(usuario);
    }

    public void RemoverUsuarioDeLaLista(Usuario usuario)
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
    
    public bool ModificarClave(string? email,string? claveActual, string? claveNueva)
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

        catalogoDeportes.ConfirmarClusters(nuevoItem1, nuevoItem);
    }
    public void AgregarCatalogo(Catalogo catalogo)
    {
        if (_catalogos.Contains(catalogo))
            throw new InvalidOperationException("Ya existe un catálogo con ese título");
        
        _catalogos.Add(catalogo);
    }

    public void CambiarTituloCatalogo(Catalogo catalogo, string titulo)
    {
        var catalogoCandidato = ObtenerCatalogoPorTitulo(titulo);
        if (catalogoCandidato is not null && !ReferenceEquals(catalogoCandidato, catalogo))
        {
            throw new InvalidOperationException("El Título del catálogo ya existe");
        }

        catalogo.CambiarTitulo(titulo);
    }
    
    public void CambiarDescripcionCatalogo(Catalogo catalogo, string? descripcion)
    {
        catalogo.CambiarDescripcion(descripcion);
    }
    
    public void EliminarCatalogo(Catalogo catalogo)
    {
        if (!_catalogos.Contains(catalogo))
            throw new InvalidOperationException("No existe un catálogo con ese título");
        
        _catalogos.Remove(catalogo);
    }
    
    public IReadOnlyCollection<Catalogo> Catalogos => _catalogos.AsReadOnly();

    public IReadOnlyCollection<Item> ObtenerItemsDelCatalogo(Catalogo catalogo) => catalogo.Items;

    public Catalogo? ObtenerCatalogoPorId(int id)
        => _catalogos.FirstOrDefault(c => c.Id == id);
    
    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
        => _catalogos.FirstOrDefault(c=> c.Titulo.Equals(titulo ?? "", StringComparison.OrdinalIgnoreCase));

    public int CantidadDeCatalogos()
    {
        return _catalogos.Count;
    }
    //------------------------------------------------------------------------
    /* Fin espacio Catalogo*/
    
   //------------------------------------------------------------------------
    // Inicio espacio Item 
    public void AltaItemConAltaDuplicados(string catalogoTitulo, Item? nuevoItem)
   {
        var catalogo = ObtenerCatalogoPorTitulo(catalogoTitulo);
        
        ValidarCatalogoYItem(catalogo, nuevoItem);

        AsegurarIdUnico(nuevoItem);
        catalogo.AgregarItem(nuevoItem);
    
        var duplicadosDelItem = DetectarDuplicados(nuevoItem, catalogo);
        AgregarDuplicadosADuplicadosGlobales(duplicadosDelItem);
        RegistrarLog(LogEntry.AccionLog.AltaItem, $"Item agregado: '{nuevoItem.Titulo}' en catálogo '{catalogoTitulo}'");

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
        
        RegistrarLog(LogEntry.AccionLog.EditarItem, $"Item actualizado: '{dto.Titulo}' en catálogo '{catalogo.Titulo}'");
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
        
        RegistrarLog(LogEntry.AccionLog.EliminarItem, $"Item eliminado: '{item.Titulo}' del catálogo '{catalogoBuscado.Titulo}'");
    }
        
    private void ValidarCatalogoYItem(Catalogo catalogo, Item item)
    {
        if (item == null || string.IsNullOrWhiteSpace(item.Titulo) || string.IsNullOrWhiteSpace(item.Descripcion))
            throw new ItemException("Título y Descripción son obligatorios.");
    }


    public void ActualizarDuplicadosPara(Catalogo? catalogo, Item? itemEditado)
    {
        if (catalogo == null || itemEditado == null)
            throw new ArgumentNullException();
        
        EliminarDuplicadosPrevios(itemEditado);
        catalogo.QuitarItemDeCluster(itemEditado);
    
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
        item.AjustarId(idApropiado);
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

    public void ConfirmarParDuplicado(ParDuplicado duplicadoConfirmado) // falta testear Mariano 
    {
        var tituloCatalogo = duplicadoConfirmado.TituloCatalogo;
        var catalogo = ObtenerCatalogoPorTitulo(tituloCatalogo);
        
        var itemEntrante = duplicadoConfirmado.ItemA;
        var itemComparado = duplicadoConfirmado.ItemB;
        
        catalogo?.ConfirmarClusters(itemEntrante,itemComparado);
        DuplicadosGlobales.Remove(duplicadoConfirmado);
        RegistrarLog(LogEntry.AccionLog.ConfirmarDuplicado, $"Se confirmó duplicado: Item '{duplicadoConfirmado.ItemA.Titulo}' y '{duplicadoConfirmado.ItemB.Titulo}'");

    }

    public void RemoverItemDelCluster(Catalogo catalogo, Item itemARemover) // testear
    {
        catalogo.QuitarItemDeCluster(itemARemover);
    }

    public void SetearNullCanonico(Cluster? cluster)
    {
        if(cluster is not null)
            cluster.Canonico = null;
    }

    public void FusionarItemsEnElCLuster(Cluster clusterAFusionar)
    {
        bool fusionado = clusterAFusionar.FusionarCanonico();
    
        if (fusionado)
        {
            RegistrarLog(LogEntry.AccionLog.FusionarCluster, 
                $"Se fusionó el canónico del cluster {clusterAFusionar.Id} con {clusterAFusionar.PertenecientesCluster.Count()} ítems.");
        }
    }

    public void DescartarParDuplicado(ParDuplicado duplicadoADescartar)
    {
        DuplicadosGlobales.Remove(duplicadoADescartar);

        duplicadoADescartar.ItemA.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemA.Id == duplicadoADescartar.ItemA.Id || unDuplicado.ItemB.Id == duplicadoADescartar.ItemA.Id);
        duplicadoADescartar.ItemB.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemA.Id == duplicadoADescartar.ItemB.Id || unDuplicado.ItemB.Id == duplicadoADescartar.ItemB.Id);
        RegistrarLog(LogEntry.AccionLog.DescartarDuplicado, $"Par duplicado descartado: '{duplicadoADescartar.ItemA.Titulo}' + '{duplicadoADescartar.ItemB.Titulo}'");

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
            LogEntry.AccionLog.DeteccionDuplicados,
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