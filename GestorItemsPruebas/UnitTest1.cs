using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios;

public class GestorItemsPruebas
{
    private static GestorItems _gestorItems = null!;
    private static GestorCatalogos _gestorCatalogos = null!;
    private static Catalogo _catalogo = null!;
    private static HashSet<int> _idsItemsGlobal = null!;
    private static List<ParDuplicado> _duplicadosGlobales = null!;

    [ClassInitialize]
    public static void Inicializar(TestContext context)
    {
        var almacenamiento = new AlmacenamientoDeDatos();
        var gestorAuditoria = new GestorAuditoria();
        var gestorDuplicados = new GestorDuplicados();

        _gestorCatalogos = new GestorCatalogos(almacenamiento);
        _idsItemsGlobal = new HashSet<int>();
        _duplicadosGlobales = new List<ParDuplicado>();

        var gestorControlDuplicados = new GestorControlDuplicados(
            gestorAuditoria,
            gestorDuplicados,
            _gestorCatalogos,
            _duplicadosGlobales
        );

        _gestorItems = new GestorItems(
            _gestorCatalogos,
            gestorControlDuplicados,
            gestorAuditoria,
            _idsItemsGlobal
        );

        _catalogo = new Catalogo("Catálogo Test");
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
        var itemDtoNoExistente = new DatosActualizarItem(
            IdCatalogo: _catalogo.Id,
            IdItem: 999,
            Titulo: "Título",
            Descripcion: "Descripcion",
            Categoria: "Cat",
            Marca: "Marca",
            Modelo: "Modelo"
        );

        var error = Assert.ThrowsException<ExcepcionItem>(
            () => _gestorItems.ActualizarItemEnCatalogo(itemDtoNoExistente)
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
    [ExpectedException(typeof(ExcepcionItem))]
    public void AltaItem_LanzaExcepcionSiTituloOVacio()
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
    [ExpectedException(typeof(ExcepcionItem))]
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
    [ExpectedException(typeof(ExcepcionItem))]
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

        Assert.AreEqual(2, _catalogo.Items.Count, "El catálogo contiene los dos ítems esperados.");
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

        Assert.AreEqual(2, _catalogo.Items.Count, "El catálogo contiene los dos ítems esperados.");
        Assert.IsTrue(item1.EstadoDuplicado, "Item1 debería estar marcado como duplicado.");
        Assert.IsTrue(item2.EstadoDuplicado, "Item2 debería estar marcado como duplicado.");
        Assert.IsTrue(_duplicadosGlobales.Count > 0, "Se generó duplicados en la lista global.");
    }

    [TestMethod]
    public void AltaItemConDuplicados_AgregaItemConIdNoValido_CorrigeElIdAutomaticamente()
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

        var item2 = _gestorItems.CrearItem(dto2);

        bool losIdsNoSonIguales = item1.Id != item2.Id;
        bool item1Existe = _idsItemsGlobal.Contains(item1.Id);
        bool item2Existe = _idsItemsGlobal.Contains(item2.Id);

        Assert.IsTrue(losIdsNoSonIguales, "Los ids no deberían ser iguales después de la corrección.");
        Assert.IsTrue(item1Existe, "El id del primer ítem debería estar registrado en la lista global.");
        Assert.IsTrue(item2Existe, "El id corregido del segundo ítem debería estar registrado en la lista global.");
    }
}
