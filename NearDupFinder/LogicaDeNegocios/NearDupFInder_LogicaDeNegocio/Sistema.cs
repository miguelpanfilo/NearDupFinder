using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.DTO;
using NearDupFInder_LogicaDeNegocio.Servicios;


namespace NearDupFinder_LogicaDeNegocio;

public class Sistema
{
    private readonly AlmacenamientoDeDatos _almacenamientoDeDatos;
    private readonly List<int> _idsItemsGlobal;
    public readonly List<ParDuplicado> DuplicadosGlobales;
    private readonly GestorUsuarios _gestorUsuarios;
    private readonly GestorDuplicados _gestorDuplicados;
    private readonly LectorCsv _lectorCsv;
    private readonly List<EntradaDeLog> _auditoria = [];
    private string _usuarioActual = "No hay usuario logueado"; 

    public Sistema()
    {
        _almacenamientoDeDatos = new AlmacenamientoDeDatos();
        _gestorUsuarios = new GestorUsuarios(this);
        _almacenamientoDeDatos.AgregarUsuario(_gestorUsuarios.CrearUsuarioAdmin());
        _gestorDuplicados = new GestorDuplicados();
        DuplicadosGlobales = new List<ParDuplicado >();
        _idsItemsGlobal = new List<int>();
        _lectorCsv = new LectorCsv(this);
    }
    
    public void AsignarUsuarioActual(string email)
    {
        _usuarioActual = email;
    }
    
    public void DesasignarUsuario()
    {
        _usuarioActual = "No hay usuario logueado";
    }
    
    public bool AltaUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaAltaDeUsuario = _gestorUsuarios.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaAltaDeUsuario)
            RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, $"Usuario: '{email}'");
        return pasaAltaDeUsuario;
    }
    
    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        bool pasaModificarUsuario = _gestorUsuarios.EditarDatosDelUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        if (pasaModificarUsuario)
            RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, $"Usuario modificado: '{email}'");
        return pasaModificarUsuario;    
    }
    
    public bool EliminarUsuario(string email)
    {
        bool pasaEliminarUsuario = _gestorUsuarios.BorrarUsuario(email);
        if (pasaEliminarUsuario)
            RegistrarLog(EntradaDeLog.AccionLog.EliminarUser, $"Usuario eliminado: '{email}'");
        return pasaEliminarUsuario;    
    }

    public IReadOnlyList<Usuario> ObtenerUsuarios()
    {
        return _almacenamientoDeDatos.ObtenerUsuarios().AsReadOnly(); 
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
        _almacenamientoDeDatos.AgregarUsuario(usuario);
    }

    public void RemoverUsuarioDeLaLista(Usuario usuario)
    {
        _almacenamientoDeDatos.RemoverUsuario(usuario);
    }
    
    public Usuario? BuscarUsuarioPorId(int id)
    {
        foreach (Usuario usuario in _almacenamientoDeDatos.ObtenerUsuarios())
            if (usuario.Id == id)
                return usuario;
        return null;    
    }
    
    public Usuario? BuscarUsuarioPorEmail(Email email)
    {
        foreach (Usuario usuario in _almacenamientoDeDatos.ObtenerUsuarios())
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
    
    public void AgregarCatalogo(Catalogo catalogo)
    {
        if (_almacenamientoDeDatos.ObtenerCatalogos().Contains(catalogo))
            throw new InvalidOperationException("Ya existe un catálogo con ese título");
        
        _almacenamientoDeDatos.AgregarCatalogo(catalogo);
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
        if (!_almacenamientoDeDatos.ObtenerCatalogos().Contains(catalogo))
            throw new InvalidOperationException("No existe un catálogo con ese título");
        
        _almacenamientoDeDatos.RemoverCatalogo(catalogo);
    }
    
    public IReadOnlyCollection<Catalogo> Catalogos => _almacenamientoDeDatos.ObtenerCatalogos().AsReadOnly();

    public IReadOnlyCollection<Item> ObtenerItemsDelCatalogo(Catalogo catalogo) => catalogo.Items;

    public Catalogo? ObtenerCatalogoPorId(int id)
        => _almacenamientoDeDatos.ObtenerCatalogos().FirstOrDefault(c => c.Id == id);
    
    public Catalogo? ObtenerCatalogoPorTitulo(string? titulo)
        => _almacenamientoDeDatos.ObtenerCatalogos().FirstOrDefault(c=> c.Titulo.Equals(titulo ?? "", StringComparison.OrdinalIgnoreCase));

    public int CantidadDeCatalogos()
    {
        return _almacenamientoDeDatos.ObtenerCatalogos().Count;
    }
    
    public void AltaItemConAltaDuplicados(string catalogoTitulo, Item? nuevoItem)
   {
        var catalogo = ObtenerCatalogoPorTitulo(catalogoTitulo);
        ValidarItem(nuevoItem);

        AsegurarIdUnico(nuevoItem);
        catalogo!.AgregarItem(nuevoItem);
    
        var duplicadosDelItem = DetectarDuplicados(nuevoItem, catalogo);
        AgregarDuplicadosADuplicadosGlobales(duplicadosDelItem);
        RegistrarLog(EntradaDeLog.AccionLog.AltaItem, $"Item agregado: '{nuevoItem.Titulo}' en catálogo '{catalogoTitulo}'");

    }
    
    public void ActualizarItemEnCatalogo(Catalogo catalogo, ItemDto dto)
    {
        var itemAEditar = catalogo.Items.FirstOrDefault(i => i.Id == dto.Id);
        if (itemAEditar == null)
            throw new ExcepcionDeItem("No se encontró el item a actualizar.");

        itemAEditar.EditarTitulo(dto.Titulo);
        itemAEditar.EditarDescripcion(dto.Descripcion);
        itemAEditar.EditarCategoria(dto.Categoria!);
        itemAEditar.EditarMarca(dto.Marca!);
        itemAEditar.EditarModelo(dto.Modelo!);
        
        RegistrarLog(EntradaDeLog.AccionLog.EditarItem, $"Item actualizado: '{dto.Titulo}' en catálogo '{catalogo.Titulo}'");
    }
    
    public void EliminarItem(string catalogo, ItemDto dto)
    {
        var catalogoBuscado = ObtenerCatalogoPorTitulo(catalogo);
        if (catalogoBuscado == null)
            throw new ArgumentException("El catálogo no existe.");

        var item = catalogoBuscado.Items.FirstOrDefault(i => i.Id == dto.Id);
        if (item == null)
            throw new ExcepcionDeItem("El item no existe en el catálogo.");

        ValidarItem(item);

        catalogoBuscado.EliminarItem(item);

        EliminarDuplicadosPrevios(item);
        ActualizarEstadoDuplicadosEnCatalogo(catalogoBuscado);
        
        RegistrarLog(EntradaDeLog.AccionLog.EliminarItem, $"Item eliminado: '{item.Titulo}' del catálogo '{catalogoBuscado.Titulo}'");
    }
    
    private void ValidarItem(Item item)
    {
        if (item == null || string.IsNullOrWhiteSpace(item.Titulo) || string.IsNullOrWhiteSpace(item.Descripcion))
            throw new ExcepcionDeItem("Título y Descripción son obligatorios.");
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
            dup.ItemAComparar.EstadoDuplicado = true;
            dup.ItemPosibleDuplicado.EstadoDuplicado = true;
        }
    }
    
    private void EliminarDuplicadosPrevios(Item item)
    {
        var duplicadosABorrar = DuplicadosGlobales
            .Where(d => d.ItemAComparar.Id == item.Id || d.ItemPosibleDuplicado.Id == item.Id)
            .ToList();
        foreach (var duplicado in duplicadosABorrar)
            DuplicadosGlobales.Remove(duplicado);
    }
    
    private void ActualizarEstadoDuplicadosEnCatalogo(Catalogo catalogo)
    {
        foreach (var item in catalogo.Items) 
        {
            bool tieneDuplicados = DuplicadosGlobales.Any(d => d.ItemAComparar.Id == item.Id || d.ItemPosibleDuplicado.Id == item.Id);
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
    
    public void ConfirmarParDuplicado(ParDuplicado duplicadoConfirmado)
    {
        var tituloCatalogo = duplicadoConfirmado.TituloCatalogo;
        var catalogo = ObtenerCatalogoPorTitulo(tituloCatalogo);
        
        var itemEntrante = duplicadoConfirmado.ItemAComparar;
        var itemComparado = duplicadoConfirmado.ItemPosibleDuplicado;
        
        catalogo?.ConfirmarClusters(itemEntrante,itemComparado);
        DuplicadosGlobales.Remove(duplicadoConfirmado);
        RegistrarLog(EntradaDeLog.AccionLog.ConfirmarDuplicado, $"Se confirmó duplicado: Item '{duplicadoConfirmado.ItemAComparar.Titulo}' y '{duplicadoConfirmado.ItemPosibleDuplicado.Titulo}'");

    }
    
    public void RemoverItemDelCluster(Catalogo catalogo, Item itemARemover) 
    {
        catalogo.QuitarItemDeCluster(itemARemover);
    }
    
    public void AsignarNuloCanonico(Cluster? cluster)
    {
        if(cluster is not null)
            cluster.Canonico = null;
    }
    
    public void FusionarItemsEnElCLuster(Cluster clusterAFusionar)
    {
        bool fusionado = clusterAFusionar.FusionarCanonico();
        if (fusionado)
        {
            RegistrarLog(EntradaDeLog.AccionLog.FusionarCluster, 
                $"Se fusionó el canónico del cluster {clusterAFusionar.Id} con {clusterAFusionar.PertenecientesCluster.Count()} ítems.");
        }
    }
    
    public bool ItemEstaEnCluster(Catalogo? catalogo, ItemDto? dto)
    {
        if (catalogo == null || dto == null)
            return false;

        var item = catalogo.Items.FirstOrDefault(i => i.Id == dto.Id);
        if (item == null)
            return false;

        var cluster = catalogo.ObtenerClusterDe(item);
        return cluster != null;
    }
    
    public void DescartarParDuplicado(ParDuplicado duplicadoADescartar)
    {
        DuplicadosGlobales.Remove(duplicadoADescartar);
        duplicadoADescartar.ItemAComparar.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemAComparar.Id == duplicadoADescartar.ItemAComparar.Id || unDuplicado.ItemPosibleDuplicado.Id == duplicadoADescartar.ItemAComparar.Id);
        duplicadoADescartar.ItemPosibleDuplicado.EstadoDuplicado = DuplicadosGlobales.Any(unDuplicado => unDuplicado.ItemAComparar.Id == duplicadoADescartar.ItemPosibleDuplicado.Id || unDuplicado.ItemPosibleDuplicado.Id == duplicadoADescartar.ItemPosibleDuplicado.Id);
        RegistrarLog(EntradaDeLog.AccionLog.DescartarDuplicado, $"Par duplicado descartado: '{duplicadoADescartar.ItemAComparar.Titulo}' + '{duplicadoADescartar.ItemPosibleDuplicado.Titulo}'");
    }
    
    public List<ParDuplicado> DetectarDuplicados(Item itemA, Catalogo catalogo)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var duplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);
        stopwatch.Stop();
        RegistrarLog(
            EntradaDeLog.AccionLog.DeteccionDuplicados,
            $"Detección de duplicados para item '{itemA.Titulo}' en catálogo '{catalogo.Titulo}' completada en {stopwatch.ElapsedMilliseconds} ms."
        );
        return duplicados;
    }
    
    public void ImportarItemsDesdeCsv(List<string> titulos, int cantidad, List<Fila> filas)
    {
        _lectorCsv.LeerCsv(titulos, cantidad, filas);
        _lectorCsv.ImportarItems();
        _lectorCsv.Limpiar();
    }

    private readonly Dictionary<EntradaDeLog.AccionLog, string> _descripcionesAccion = new()
    {
        {EntradaDeLog.AccionLog.AltaUsuario, "Creacion de usuario"},
        {EntradaDeLog.AccionLog.EditarUsuario, "Modificacion de usuario"},
        {EntradaDeLog.AccionLog.AltaItem, "Alta de item"},
        {EntradaDeLog.AccionLog.EliminarItem, "Eliminación de item"},
        {EntradaDeLog.AccionLog.DeteccionDuplicados, "Detección duplicados automatica"},
        {EntradaDeLog.AccionLog.ConfirmarDuplicado ,"Confirmación de duplicado"},
        {EntradaDeLog.AccionLog.FusionarCluster,"Fusión de cluster"},
        {EntradaDeLog.AccionLog.DescartarDuplicado,"Descartar duplicado"},
        {EntradaDeLog.AccionLog.EditarItem,"Editar item"},
        {EntradaDeLog.AccionLog.EliminarUser,"Eliminacion de usuario"},
    };
    
    public void RegistrarLog(EntradaDeLog.AccionLog accion, string? detalles)
    {
        var entry = new EntradaDeLog
        {
            Timestamp = DateTime.UtcNow,
            Usuario = _usuarioActual,
            Accion = accion,
            Detalles = $"{_descripcionesAccion[accion]}: {detalles}"
        };
        _auditoria.Add(entry);
    }
    
    public IReadOnlyList<EntradaDeLog> ObtenerLogs() => _auditoria.AsReadOnly();
    
    
  
}