using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFInder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_Pruebas.Servicios.Items;

[TestClass]
public class GestorItemPruebas
{
    private ControladorItems _controladorItems = null!;
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

        var gestorControlDuplicados = new ControladorDuplicados(
            _gestorAuditoria,
            gestorDuplicados,
            _gestorCatalogos,
            _duplicadosGlobales
        );

        _gestorItems = new GestorItems(
            _idsItemsGlobal
        );

        _catalogo = new Catalogo("Catálogo Auditoría Test");
        almacenamiento.AgregarCatalogo(_catalogo);
       
        _controladorItems = new ControladorItems(
            _gestorItems,
            _gestorCatalogos,
            gestorControlDuplicados,
            _gestorAuditoria,
            _idsItemsGlobal
        );
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

        _controladorItems.ActualizarItemEnCatalogo(dto);

        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }
    
    [TestMethod]
    public void CantidadDeItemsGlobal_SinItems_RetornaCero()
    {
        int numeroDeItems = _idsItemsGlobal.Count;
        Assert.AreEqual(0, numeroDeItems);
    }

    [TestMethod]
    public void CantidadDeItemsGlobal_ConItems_RetornaDistintoDeCero()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Item 1",
            Descripcion: "Descripción 1"
        );

        _controladorItems.CrearItem(dto);

        int numeroDeItems = _idsItemsGlobal.Count;

        Assert.AreNotEqual(0, numeroDeItems);
        Assert.AreEqual(1, numeroDeItems);
    }
    [TestMethod]
    public void IdExisteEnListaDeIdGlobal_ConItemNoExistente_RetornaFalso()
    {
        var nuevoItem = new Item("Item 1", "Descripción 1");

        bool existeItem = _gestorItems.IdExisteEnListaDeIdGlobal(nuevoItem.Id);

        Assert.IsFalse(existeItem);
    }

    
    [TestMethod]
    public void IdExisteEnListaDeIdGlobal_ConItemExistente_RetornaVerdadero()
    {
        var dto = new DatosCrearItem(
            IdCatalogo: _catalogo.Id,
            Titulo: "Item 1",
            Descripcion: "Descripción 1"
        );

        var item = _controladorItems.CrearItem(dto);

        bool existeItem = _gestorItems.IdExisteEnListaDeIdGlobal(item.Id);

        Assert.IsTrue(existeItem);
    }
}
  