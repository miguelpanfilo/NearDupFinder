using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_Pruebas.Servicios.Importacion;

[TestClass]
public class GestorLectorCsvPruebas
{
    private GestorLectorCsv _gestorLectorCsv = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorItems _gestorItems = null!;
    private AlmacenamientoDeDatos _almacenamientoDeDatos = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorControlClusters _gestorControlClusters = null!;
    private List<ParDuplicado> _duplicadosGlobales = [];
    private GestorDuplicados _gestorDuplicados = null!;
    private readonly HashSet<int> _idsItemsGlobal = [];
    private ControladorItems _controladorItems = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _almacenamientoDeDatos = new AlmacenamientoDeDatos();
        _gestorCatalogos = new GestorCatalogos(_almacenamientoDeDatos);
        _gestorAuditoria = new GestorAuditoria();
        _gestorDuplicados = new GestorDuplicados();
        _gestorControlClusters = new GestorControlClusters(_gestorCatalogos,_gestorAuditoria);
        _controladorDuplicados = new ControladorDuplicados(_gestorAuditoria, _gestorDuplicados,_gestorCatalogos, _gestorControlClusters ,_duplicadosGlobales);
        _gestorItems = new GestorItems(_idsItemsGlobal);
        _controladorItems = new ControladorItems(_gestorItems,_gestorCatalogos,_controladorDuplicados,_gestorAuditoria, _idsItemsGlobal);
        _gestorLectorCsv = new GestorLectorCsv(_gestorCatalogos, _gestorItems,_controladorItems);
    }
    
    [TestMethod]
    public void Constructor_InicializaColeccionesVacias()
    {
        Assert.IsNotNull(_gestorLectorCsv.Titulos);
        Assert.IsNotNull(_gestorLectorCsv.Filas);
        Assert.AreEqual(0, _gestorLectorCsv.Titulos.Count);
        Assert.AreEqual(0, _gestorLectorCsv.Filas.Count);
        Assert.AreEqual(0, _gestorLectorCsv.CantidadDeFilas);
    }

    [TestMethod]
    public void LeerCsv_CargaTitulosCantidadYFilas()
    {
        List<string> titulos = new List<string> { "id", "titulo" };
        List<Fila> filas = new List<Fila> { new Fila("1","t","m","x","d","c","Cat 1") };

        _gestorLectorCsv.LeerCsv(titulos, 1, filas);

        CollectionAssert.AreEqual(titulos, _gestorLectorCsv.Titulos);
        Assert.AreEqual(1, _gestorLectorCsv.CantidadDeFilas);
        Assert.AreEqual(1, _gestorLectorCsv.Filas.Count);
        Assert.AreEqual("1", _gestorLectorCsv.Filas[0].Id);
    }

    [TestMethod]
    public void Limpiar_ReiniciaEstado()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id" }, 1,
            new List<Fila> { new Fila("1","t","m","x","d","c","Cat") });

        _gestorLectorCsv.Limpiar();

        Assert.AreEqual(0, _gestorLectorCsv.Titulos.Count);
        Assert.AreEqual(0, _gestorLectorCsv.Filas.Count);
        Assert.AreEqual(0, _gestorLectorCsv.CantidadDeFilas);
    }

    [TestMethod]
    public void ImportarItems_CatalogoVacio_SaltaFila()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("10","t","m","x","d","c","") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsNull(_gestorCatalogos.ObtenerCatalogoPorTitulo(""));
    }

    [TestMethod]
    public void ImportarItems_CreaCatalogoSiNoExiste()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("1","t","m","x","d","c","Cat Nuevo") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsNotNull(_gestorCatalogos.ObtenerCatalogoPorTitulo("Cat Nuevo"));
    }

    [TestMethod]
    public void ImportarItems_RespetaIdNuevo()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("42","t","m","x","d","c","Cat A") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsTrue(_gestorItems.IdExisteEnListaDeIdGlobal(42));
    }

    [TestMethod]
    public void ImportarItems_IdVacio_NoRegistraIdCero()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("","t","m","x","d","c","Cat Z") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsFalse(_gestorItems.IdExisteEnListaDeIdGlobal(0));
    }

    [TestMethod]
    public void ImportarItems_ItemInvalido_NoRegistraId_PuedeCrearCatalogo()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("1","", "m","x","", "c","Cat Invalido") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsNotNull(_gestorCatalogos.ObtenerCatalogoPorTitulo("Cat Invalido"));
        Assert.IsFalse(_gestorItems.IdExisteEnListaDeIdGlobal(1));
    }
    
    [TestMethod]
    public void ImportarItems_IdDuplicado_NoRegistraSegundoItem()
    {
        _gestorLectorCsv.LeerCsv(
            new List<string> { "id", "titulo" }, 
            2,
            new List<Fila>
            {
                new Fila("5", "t1", "m", "x", "d", "c", "Cat D"),
                new Fila("5", "t2", "m", "x", "d", "c", "Cat D")
            }
        );

        _gestorLectorCsv.ImportarItems();
        Catalogo catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat D")!;
        
        Assert.IsTrue(_gestorItems.IdExisteEnListaDeIdGlobal(5));
        Assert.AreEqual("t1", catalogo.ObtenerItemPorId(5)!.Titulo);
    }
    
    [TestMethod]
    public void ImportarItems_CatalogoInvalido_MasDe120Caracteres_SaltaFilaSinRegistrarIdNiCatalogo()
    {
        string nombreLargo = new string('X', 121);
        _gestorLectorCsv.LeerCsv(new List<string> { "id", "titulo" }, 1,
            new List<Fila> { new Fila("123", "t", "m", "x", "d", "c", nombreLargo) });

        _gestorLectorCsv.ImportarItems();

        Assert.IsNull(_gestorCatalogos.ObtenerCatalogoPorTitulo(nombreLargo));
        Assert.IsFalse(_gestorItems.IdExisteEnListaDeIdGlobal(123));
    }
}