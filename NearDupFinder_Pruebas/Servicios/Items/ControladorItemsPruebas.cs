using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Items;

[TestClass]
public class ControladorItemsPruebas
{
    private ControladorItems _controladorItems = null!;
    private GestorItems _gestorItems = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorControlClusters _gestorControlClusters = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    private GestorDuplicados _gestorDuplicados = null!;
    private Catalogo _catalogo = null!;
    private HashSet<int> _idsItemsGlobal = null!;
    private IRepositorioDuplicados _repoDuplicados = null!;
    private SqlContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var procesador = new ProcesadorTexto();

        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_ControladorItems");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

        IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);
        IRepositorioItems repoItems = new RepositorioItems(_context);
        IRepositorioClusters repoClusters = new RepositorioClusters(_context);
        IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context);

        var sesionUsuario = new SesionUsuarioActual();
        sesionUsuario.Asignar("tester@correo.com");

        _gestorAuditoria = new GestorAuditoria(repoAuditorias, sesionUsuario);
        _gestorCatalogos = new GestorCatalogos(repoCatalogos, repoClusters, repoItems);
        _gestorDuplicados = new GestorDuplicados(procesador);

        _gestorControlClusters = new GestorControlClusters(
            _gestorCatalogos,
            _gestorAuditoria,
            repoCatalogos,
            repoClusters,
            repoItems
        );

        _idsItemsGlobal = new HashSet<int>();
        _gestorItems = new GestorItems(repoItems);

        _repoDuplicados = new RepositorioDuplicados(_context);

        _controladorDuplicados = new ControladorDuplicados(
            _gestorAuditoria,
            _gestorDuplicados,
            _gestorCatalogos,
            _gestorControlClusters,
            _repoDuplicados
        );


        _controladorItems = new ControladorItems(
            _gestorItems,
            _gestorCatalogos,
            _controladorDuplicados,
            _gestorControlClusters,
            _gestorAuditoria);

        _catalogo = new Catalogo("Catálogo Auditoría Test");
        repoCatalogos.Agregar(_catalogo);
        repoCatalogos.GuardarCambios();
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
    {
        var item = new Item("Original", "Descripción original", 0)
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };

        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        _context.Items.Add(item);
        _context.SaveChanges();

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Titulo: "Nuevo Título",
            Descripcion: "Nueva Descripción",
            Categoria: "Cat 1",
            Marca: "Marca 1",
            Modelo: "Modelo 1"
        );

        _controladorItems.ActualizarItemEnCatalogo(dto);

        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
    {
        var item = new Item("Original", "Descripcion original", 0)
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };

        _catalogo.AgregarItem(item);
        _context.Items.Add(item);
        _context.SaveChanges();

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Categoria: "Cat 2",
            Marca: "Marca 2",
            Modelo: "Modelo 2"
        );

        _controladorItems.ActualizarItemEnCatalogo(dto);

        Assert.AreEqual("Cat 2", item.Categoria);
        Assert.AreEqual("Marca 2", item.Marca);
        Assert.AreEqual("Modelo 2", item.Modelo);
    }


    [TestMethod]
    public void ActualizarItemEnCatalogo_ItemNoExiste_Excepcion()
    {
        int catalogoExistenteId = _catalogo.Id;
        const int idItemInexistente = 9999;

        var itemDtoNoExistente = new DatosActualizarItem(
            IdCatalogo: catalogoExistenteId,
            IdItem: idItemInexistente,
            Titulo: "Título",
            Descripcion: "Descripcion",
            Categoria: "Cat",
            Marca: "Marca",
            Modelo: "Modelo"
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorItems.ActualizarItemEnCatalogo(itemDtoNoExistente)
        );

        Assert.AreEqual($"Ítem no encontrado (Id={itemDtoNoExistente.IdItem}).", error.Message);
    }


    [TestMethod]
    public void AltaItem_AgregaItemAlCatalogo()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: "Descripcion"
        );

        _controladorItems.CrearItem(dto);

        var items = _catalogo.Items;

        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Titulo", items.First().Titulo);
        Assert.AreEqual("Descripcion", items.First().Descripcion);
    }


    [TestMethod]
    public void AltaItem_TituloVacio_Excepcion()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "",
            Descripcion: "Descripcion"
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() => _controladorItems.CrearItem(dto));

        Assert.AreEqual("Título y Descripción son obligatorios.", error.Message);
    }

    [TestMethod]
    public void AltaItem_DescripcionVacia_Excepcion()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: ""
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() => _controladorItems.CrearItem(dto));

        Assert.AreEqual("Título y Descripción son obligatorios.", error.Message);
    }


    [TestMethod]
    public void AltaItem_Nulo_Excepcion()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: null,
            Descripcion: null
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() => _controladorItems.CrearItem(dto));

        Assert.AreEqual("Título y Descripción son obligatorios.", error.Message);
    }

    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnBD()
    {
        var dto1 = new DatosCrearItem(_catalogo.Id, "Titulo 1", "Descripcion 1");
        _controladorItems.CrearItem(dto1);

        var dto2 = new DatosCrearItem(_catalogo.Id, "Titulo 1", "Descripcion 1");
        _controladorItems.CrearItem(dto2);

        var duplicados = _repoDuplicados.ObtenerListaDeDuplicados();

        Assert.AreEqual(2, _catalogo.Items.Count, "El catálogo debe contener ambos ítems.");
        Assert.AreEqual(1, duplicados.Count, "Debe haberse generado exactamente un duplicado en BD.");
    }

    [TestMethod]
    public void AltaItemConDuplicados_ItemTieneDuplicado_EstadoDuplicadoEsTrue()
    {
        var dto1 = new DatosCrearItem(_catalogo.Id, "Titulo 1", "Descripcion 1");
        var dto2 = new DatosCrearItem(_catalogo.Id, "Titulo 1", "Descripcion 1");

        var item1 = _controladorItems.CrearItem(dto1);
        var item2 = _controladorItems.CrearItem(dto2);

        var duplicados = _repoDuplicados.ObtenerListaDeDuplicados();

        Assert.AreEqual(2, _catalogo.Items.Count);
        Assert.IsTrue(item1.EstadoDuplicado);
        Assert.IsTrue(item2.EstadoDuplicado);
        Assert.IsTrue(duplicados.Count > 0, "Debe existir al menos un par duplicado persistido.");
    }

    [TestMethod]
    public void AltaItemConDuplicados_AgregaItemConIdNoValido_LanzaExcepcion()
    {
        var dto1 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: "Descripcion"
        );

        var item1 = _controladorItems.CrearItem(dto1);
        int idItem1 = item1.Id;

        var dto2 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: "Descripcion",
            IdImportado: idItem1
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() => _controladorItems.CrearItem(dto2)
        );

        Assert.AreEqual(
            $"El Id importado {idItem1} ya existe.",
            error.Message
        );
    }

    [TestMethod]
    public void EliminarItemYActualizarEstadoDuplicados_Correcto()
    {
        var dto1 = new DatosCrearItem(_catalogo.Id, "Titulo", "Descripcion");
        var dto2 = new DatosCrearItem(_catalogo.Id, "Titulo", "Descripcion");

        var item1 = _controladorItems.CrearItem(dto1);
        var item2 = _controladorItems.CrearItem(dto2);

        Assert.IsTrue(_repoDuplicados.ObtenerListaDeDuplicados().Any());

        _controladorItems.EliminarItem(new DatosEliminarItem(_catalogo.Id, item1.Id));

        Assert.IsFalse(item2.EstadoDuplicado, "El item duplicado debe quedar en false tras eliminar el otro.");
        Assert.IsFalse(_repoDuplicados.ObtenerListaDeDuplicados().Any(), "El par duplicado debe haberse eliminado.");
    }

    [TestMethod]
    public void EliminarItem_ItemNoExistente_DeberiaLanzarExcepcionConMensaje_Correcto()
    {
        const int idItemInexistente = 9999;
        var dtoInexistente = new DatosEliminarItem(_catalogo.Id, idItemInexistente);

        var ex = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorItems.EliminarItem(dtoInexistente)
        );

        Assert.AreEqual("El item no existe en el catálogo.", ex.Message);
    }


    [TestMethod]
    public void EliminarItem_CatalogoNoExistente_DeberiaLanzarExcepcionCatalogoConMensaje_Correcto()
    {
        const int idCatalogoInexistente = 9999;
        const int idItemExistente = 1;
        var dtoInexistente = new DatosEliminarItem(idCatalogoInexistente, idItemExistente);


        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _controladorItems.EliminarItem(dtoInexistente)
        );


        Assert.AreEqual("El catálogo no existe.", ex.Message);
    }

    [TestMethod]
    public void EliminarItem_EliminaItemDelCatalogo()
    {
        Item item1 = Item.Crear("Item 1", "Desc 1", "Categoria", "Marca", "Modelo");
        Item item2 = Item.Crear("Item 2", "Desc 2", "Categoria", "Marca", "Modelo");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        _context.Items.AddRange(item1, item2);
        _context.SaveChanges();

        var dtoEliminar = new DatosEliminarItem(
            idCatalogo: _catalogo.Id,
            idItem: item1.Id
        );

        _controladorItems.EliminarItem(dtoEliminar);

        Assert.IsFalse(_catalogo.Items.Contains(item1), "Item1 debe ser eliminado del catálogo.");
        Assert.IsTrue(_catalogo.Items.Contains(item2), "Item2 debe estar en el catálogo.");
    }

    [TestMethod]
    public void CrearItem_DeberiaRegistrarLogDeAlta_Correcto()
    {
        const int cantidadValidaDeAuditoriasRegistradas = 1;
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Nuevo Item",
            Descripcion: "Descripción del item"
        );

        var itemCreado = _controladorItems.CrearItem(dto);

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.IsTrue(logs.Count >= cantidadValidaDeAuditoriasRegistradas,
            "Debe haberse registrado al menos un log por la creación del ítem.");

        var logAlta = logs.FirstOrDefault(l => l.Accion == EntradaDeLog.AccionLog.AltaItem);
        Assert.IsNotNull(logAlta, "Debe existir un log de AltaItem.");

        var detalleEsperado =
            $"Alta de ítem: Item agregado: '{itemCreado.Titulo}' en catálogo '{_catalogo.Titulo}'.";

        Assert.AreEqual(detalleEsperado, logAlta.Detalles);
    }


    [TestMethod]
    public void ActualizarItem_DeberiaRegistrarLogDeEdicion()
    {
        var item = new Item("Original", "Desc original", 0);
        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        _context.Items.Add(item);
        _context.SaveChanges();

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Titulo: "Editado",
            Descripcion: "Descripcion editada"
        );

        _controladorItems.ActualizarItemEnCatalogo(dto);

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        var logsEditar = logs
            .Where(l => l.Accion == EntradaDeLog.AccionLog.EditarItem)
            .ToList();

        Assert.AreEqual(1, logsEditar.Count, "Debe registrarse exactamente 1 log de edición.");
        StringAssert.Contains(logsEditar[0].Detalles, "Ítem actualizado");
    }


    [TestMethod]
    public void EliminarItem_DeberiaRegistrarLogDeEliminacion()
    {
        var item = new Item("A borrar", "Descripción del item", 0);
        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        _context.Items.Add(item);
        _context.SaveChanges();

        var dtoEliminar = new DatosEliminarItem(
            idCatalogo: _catalogo.Id,
            idItem: item.Id
        );

        _controladorItems.EliminarItem(dtoEliminar);

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();
        const int primeraAuditoriaRegistrada = 0;

        Assert.AreEqual(EntradaDeLog.AccionLog.EliminarItem, logs[primeraAuditoriaRegistrada].Accion);
        StringAssert.Contains(logs[primeraAuditoriaRegistrada].Detalles, "Item eliminado");
    }


    [TestMethod]
    public void AgregarDuplicado_SePersisteEnBaseDeDatos()
    {
        Item itemA = Item.Crear("Notebook", "Intel i5", "Categoria", "Marca", "Modelo");
        Item itemB = Item.Crear("Notebook", "Intel i5", "Categoria", "Marca", "Modelo");

        _context.Items.AddRange(itemA, itemB);
        _context.SaveChanges();

        var par = new ParDuplicado(itemA, itemB, 0.9f, TipoDuplicado.PosibleDuplicado, 1, 1, 1, 1,
            new[] { "notebook" }, new[] { "intel" }, _catalogo.Id);
        _repoDuplicados.AgregarDuplicado(par);

        var duplicados = _repoDuplicados.ObtenerListaDeDuplicados();
        Assert.AreEqual(1, duplicados.Count);
    }
    [TestMethod]
    public void ObtenerItemsDelCatalogo_SinItems_DevuelveListaVacia()
    {
        var resultado = _controladorItems.ObtenerItemsDelCatalogo(_catalogo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }
    [TestMethod]
    public void ObtenerItemsDelCatalogo_ConItems_DevuelveItemsCorrectos()
    {
        var item1 = Item.Crear("Item A", "Desc A");
        var item2 = Item.Crear("Item B", "Desc B");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        _context.Items.AddRange(item1, item2);
        _context.SaveChanges();

        var resultado = _controladorItems.ObtenerItemsDelCatalogo(_catalogo.Id);

        Assert.AreEqual(2, resultado.Count);

        Assert.IsTrue(resultado.Any(i => i.Id == item1.Id && i.Titulo == "Item A"));
        Assert.IsTrue(resultado.Any(i => i.Id == item2.Id && i.Titulo == "Item B"));
    }
    [TestMethod]
    public void ObtenerItemsDelCatalogo_CatalogoInexistente_LanzaExcepcion()
    {
        const int catalogoInexistente = 9999;

        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _controladorItems.ObtenerItemsDelCatalogo(catalogoInexistente)
        );

        Assert.AreEqual($"Catálogo no encontrado (Id={catalogoInexistente}).", ex.Message);
    }




}

  

