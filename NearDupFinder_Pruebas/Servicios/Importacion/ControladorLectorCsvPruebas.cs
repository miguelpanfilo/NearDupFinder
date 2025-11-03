using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_Pruebas.Servicios.Importacion;

[TestClass]
public class ControladorLectorCsvPruebas
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
    private ControladorLectorCsv _controladorLectorCsv = null!;
    private ControladorItems _controladorItems = null!;

    
    [TestInitialize]
    public void Setup()
    {
        _almacenamientoDeDatos = new AlmacenamientoDeDatos();
        _gestorCatalogos = new GestorCatalogos(_almacenamientoDeDatos);
        _gestorAuditoria = new GestorAuditoria();
        _gestorDuplicados = new GestorDuplicados();
        _gestorControlClusters = new GestorControlClusters(_gestorCatalogos,_gestorAuditoria);
        _controladorDuplicados = new ControladorDuplicados(_gestorAuditoria, _gestorDuplicados,_gestorCatalogos, _gestorControlClusters, _duplicadosGlobales);
        _gestorItems = new GestorItems(_idsItemsGlobal);
        _controladorItems = new ControladorItems(_gestorItems,_gestorCatalogos,_controladorDuplicados,_gestorControlClusters,_gestorAuditoria, _idsItemsGlobal);

        _gestorLectorCsv = new GestorLectorCsv(_gestorCatalogos, _gestorItems,_controladorItems);
        _controladorLectorCsv = new ControladorLectorCsv(_gestorLectorCsv);
    }
    
    [TestMethod]
    public void ImportarItemsDesdeCsv_CreaCatalogoEImportaUnItem()
    {
        List<string> titulos = new List<string> { "id", "titulo" };
        List<Fila> filas = new List<Fila> {
            new Fila("101", "notebook", "lenovo", "l14", "intel i5", "notebooks", "Cat Import")
        };
        DatosImportarCsv datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Catalogo catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat Import")!;
        Assert.IsNotNull(catalogo);
        Assert.AreEqual(1, catalogo.Items.Count);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_RegistraIdGlobal()
    {
        List<string> titulos = new List<string> { "id", "titulo" };
        List<Fila> filas = new List<Fila> {
            new Fila("101", "notebook", "lenovo", "l14", "intel i5", "notebooks", "Cat Import")
        };
        DatosImportarCsv datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.IsTrue(_gestorItems.IdExisteEnListaDeIdGlobal(101));
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_LimpiaListasDelLector()
    {
        List<string> titulos = new List<string> { "id", "titulo" };
        List<Fila> filas = new List<Fila> {
            new Fila("1", "t", "m", "x", "d", "c", "Cat")
        };
        DatosImportarCsv datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.AreEqual(0, _gestorLectorCsv.Titulos.Count);
        Assert.AreEqual(0, _gestorLectorCsv.Filas.Count);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_ReseteaCantidadDeFilas()
    {
        List<string> titulos = new List<string> { "id", "titulo" };
        List<Fila> filas = new List<Fila> {
            new Fila("1", "t", "m", "x", "d", "c", "Cat")
        };
        DatosImportarCsv datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.AreEqual(0, _gestorLectorCsv.CantidadDeFilas);
    }
    
}