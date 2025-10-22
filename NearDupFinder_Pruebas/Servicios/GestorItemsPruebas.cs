using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios;

[TestClass]
public class GestorItemsPruebas
{
    private GestorItems _gestorItems = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private Catalogo _catalogo = null!;
    private HashSet<int> _idsItemsGlobal = null!;
    private List<ParDuplicado> _duplicadosGlobales = null!;

    [TestInitialize]
    public void Setup()
    {
        var almacenamiento = new AlmacenamientoDeDatos();
        _gestorAuditoria = new GestorAuditoria();
        var gestorDuplicados = new GestorDuplicados();

        _gestorCatalogos = new GestorCatalogos(almacenamiento);
        _idsItemsGlobal = new HashSet<int>();
        _duplicadosGlobales = new List<ParDuplicado>();

        var gestorControlDuplicados = new GestorControlDuplicados(
            _gestorAuditoria,
            gestorDuplicados,
            _gestorCatalogos,
            _duplicadosGlobales
        );

        _gestorItems = new GestorItems(
            _gestorCatalogos,
            gestorControlDuplicados,
            _gestorAuditoria,
            _idsItemsGlobal
        );

        _catalogo = new Catalogo("Catálogo Auditoría Test");
        almacenamiento.AgregarCatalogo(_catalogo);
    }
    
    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
    {
        var item = new Item("Original", "Descripción original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };

        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Titulo: "Nuevo Título",
            Descripcion: "Nueva Descripción",
            Categoria: "Cat 1",
            Marca: "Marca 1",
            Modelo: "Modelo 1"
        );

        _gestorItems.ActualizarItemEnCatalogo(dto);

        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
    {
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };

        _catalogo.AgregarItem(item);

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Categoria: "Cat 2",
            Marca: "Marca 2",
            Modelo: "Modelo 2"
        );

        _gestorItems.ActualizarItemEnCatalogo(dto);

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
            _gestorItems.ActualizarItemEnCatalogo(itemDtoNoExistente)
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

        _gestorItems.CrearItem(dto);

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

        var error = Assert.ThrowsException<ExcepcionItem>(() => _gestorItems.CrearItem(dto));

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

        var error = Assert.ThrowsException<ExcepcionItem>(() => _gestorItems.CrearItem(dto));

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

        var error = Assert.ThrowsException<ExcepcionItem>(() => _gestorItems.CrearItem(dto));

        Assert.AreEqual("Título y Descripción son obligatorios.", error.Message);
    }

    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnListaGlobal()
    {
        var dto1 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo 1",
            Descripcion: "Descripcion 1"
        );
        _gestorItems.CrearItem(dto1);

        var dto2 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo 1",
            Descripcion: "Descripcion 1"
        );
        _gestorItems.CrearItem(dto2);
        const int cantidadDeItemEnElCatalogo = 2;


        Assert.AreEqual(cantidadDeItemEnElCatalogo, _catalogo.Items.Count, "El catálogo contiene los dos ítems esperados.");
        Assert.IsTrue(_duplicadosGlobales.Count == 1, "No se generó ningún duplicado en la lista global.");
    }

    [TestMethod]
    public void AltaItemConDuplicados_ItemTieneDuplicado_EstadoDuplicadoEsTrue()
    {
        var dto1 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo 1",
            Descripcion: "Descripcion 1"
        );

        var dto2 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo 1",
            Descripcion: "Descripcion 1"
        );

        var item1 = _gestorItems.CrearItem(dto1);
        var item2 = _gestorItems.CrearItem(dto2);
        const int cantidadDeItemEnElCatalogo = 2;

        const int duplicadosGlobalesVacio = 0;

        Assert.AreEqual(cantidadDeItemEnElCatalogo, _catalogo.Items.Count, "El catálogo contiene los dos ítems esperados.");
        Assert.IsTrue(item1.EstadoDuplicado, "Item1 debería estar marcado como duplicado.");
        Assert.IsTrue(item2.EstadoDuplicado, "Item2 debería estar marcado como duplicado.");
        Assert.IsTrue(_duplicadosGlobales.Count > duplicadosGlobalesVacio);
    }

  
    [TestMethod]
    public void AltaItemConDuplicados_AgregaItemConIdNoValido_LanzaExcepcion()
    {
        var dto1 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: "Descripcion"
        );

        var item1 = _gestorItems.CrearItem(dto1);
        int idItem1 = item1.Id;

        var dto2 = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Titulo",
            Descripcion: "Descripcion",
            IdImportado: idItem1
        );

        var error = Assert.ThrowsException<ExcepcionItem>(() => _gestorItems.CrearItem(dto2)
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

        var item1 = _gestorItems.CrearItem(dto1);
        var item2 = _gestorItems.CrearItem(dto2);

        _gestorItems.EliminarItem(new DatosEliminarItem(_catalogo.Id, item1.Id));

        Assert.IsFalse(item2.EstadoDuplicado, "El item duplicado debería dejar de estar marcado en false después de eliminar el item1.");
    }

    [TestMethod]
    public void EliminarItem_ItemNoExistente_DeberiaLanzarExcepcionConMensaje_Correcto()
    {
        const int idItemInexistente=9999;
        var dtoInexistente = new DatosEliminarItem(_catalogo.Id, idItemInexistente);

        var ex = Assert.ThrowsException<ExcepcionItem>(() =>
            _gestorItems.EliminarItem(dtoInexistente)
        );

        Assert.AreEqual("El item no existe en el catálogo.", ex.Message);
    }


    [TestMethod]
    public void EliminarItem_CatalogoNoExistente_DeberiaLanzarExcepcionCatalogoConMensaje_Correcto()
    {
        const int idCatalogoInexistente = 9999;
        const int idItemExistente=1;
        var dtoInexistente = new DatosEliminarItem(idCatalogoInexistente,idItemExistente);


        var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _gestorItems.EliminarItem(dtoInexistente)
        );
        

        Assert.AreEqual("El catálogo no existe.", ex.Message);
    }

    [TestMethod]
    public void EliminarItem_EliminaItemDelCatalogo()
    {
        var item1 = new Item("Item 1", "Desc 1");
        var item2 = new Item("Item 2", "Desc 2");

        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        var dtoEliminar = new DatosEliminarItem(
            idCatalogo: _catalogo.Id,
            idItem: item1.Id
        );

        _gestorItems.EliminarItem(dtoEliminar);

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

        var itemCreado = _gestorItems.CrearItem(dto);

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.IsTrue(logs.Count >= cantidadValidaDeAuditoriasRegistradas, "Debe haberse registrado al menos un log por la creación del ítem.");

        var logAlta = logs.FirstOrDefault(l => l.Accion == EntradaDeLog.AccionLog.AltaItem);
        Assert.IsNotNull(logAlta, "Debe existir un log de AltaItem.");

        var detalleEsperado = $"Alta de item: Item agregado: '{itemCreado.Titulo}' (Id={itemCreado.Id}) en catálogo '{_catalogo.Titulo}' (Id={_catalogo.Id}).";

        Assert.AreEqual(detalleEsperado, logAlta.Detalles);
    }


    [TestMethod]
    public void ActualizarItem_DeberiaRegistrarLogDeEdicion()
    {
        var item = new Item("Original", "Desc original");
        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        var dto = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: item.Id,
            Titulo: "Editado",
            Descripcion: "Descripcion editada"
        );

        _gestorItems.ActualizarItemEnCatalogo(dto);

        var logs = _gestorAuditoria.ObtenerLogs();
        const int primeraAuditoriaregistrada = 0;
        const int cantidadDeAuditoria = 1;

        Assert.AreEqual(cantidadDeAuditoria, logs.Count);
        Assert.AreEqual(EntradaDeLog.AccionLog.EditarItem, logs[primeraAuditoriaregistrada].Accion);
        StringAssert.Contains(logs[primeraAuditoriaregistrada].Detalles, "Ítem actualizado");
    }

    [TestMethod]
    public void EliminarItem_DeberiaRegistrarLogDeEliminacion()
    {
        var item = new Item("A borrar", "Descripción del item");
        _catalogo.AgregarItem(item);
        _idsItemsGlobal.Add(item.Id);

        var dtoEliminar = new DatosEliminarItem(
            idCatalogo: _catalogo.Id,
            idItem: item.Id
        );

        _gestorItems.EliminarItem(dtoEliminar);

        var logs = _gestorAuditoria.ObtenerLogs();
        const int primeraAuditoriaRegistrada = 0;
      
        Assert.AreEqual(EntradaDeLog.AccionLog.EliminarItem, logs[primeraAuditoriaRegistrada].Accion);
        StringAssert.Contains(logs[primeraAuditoriaRegistrada].Detalles, "Item eliminado");
   
    }



}
  

