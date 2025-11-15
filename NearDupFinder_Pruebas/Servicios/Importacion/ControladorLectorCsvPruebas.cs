using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Importacion;

[TestClass]
public class ControladorLectorCsvPruebas
{
    private GestorLectorCsv _gestorLectorCsv = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorItems _gestorItems = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorControlClusters _gestorControlClusters = null!;
    private GestorDuplicados _gestorDuplicados = null!;
    private ControladorLectorCsv _controladorLectorCsv = null!;
    private ControladorItems _controladorItems = null!;
    private SqlContext _context = null!;

    private readonly HashSet<int> _idsItemsGlobal = new();     
    private IRepositorioDuplicados _repoDuplicados = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _idsItemsGlobal.Clear();

        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_LectorCsv");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

        var sesionUsuario = new SesionUsuarioActual();
        sesionUsuario.Asignar("tester@correo.com");
        var procesador = new ProcesadorTexto();

        IRepositorioCatalogos repoCatalogos   = new RepositorioCatalogos(_context);
        IRepositorioItems repoItems           = new RepositorioItems(_context);
        IRepositorioClusters repoClusters     = new RepositorioClusters(_context);
        IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context);
        _repoDuplicados = new RepositorioDuplicados(_context);

        _gestorAuditoria  = new GestorAuditoria(repoAuditorias, sesionUsuario);
        _gestorCatalogos  = new GestorCatalogos(repoCatalogos);
        _gestorDuplicados = new GestorDuplicados(procesador);

        _gestorControlClusters = new GestorControlClusters(
            _gestorCatalogos,
            _gestorAuditoria,
            repoCatalogos,
            repoClusters,
            repoItems
        );

        _gestorItems = new GestorItems(repoItems);

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

        _gestorLectorCsv = new GestorLectorCsv(_gestorCatalogos, _gestorItems, _controladorItems);
        _controladorLectorCsv = new ControladorLectorCsv(_gestorLectorCsv);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_CreaCatalogoEImportaUnItem()
    {
        List<string> titulos = new() { "id", "titulo" };
        List<Fila> filas = new() {
            new Fila("101", "notebook", "lenovo", "l14", "intel i5", "notebooks", "Cat Import")
        };
        var datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Catalogo catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Cat Import")!;
        Assert.IsNotNull(catalogo);
        Assert.AreEqual(1, catalogo.Items.Count);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_RegistraIdGlobal()
    {
        List<string> titulos = new() { "id", "titulo" };
        List<Fila> filas = new() {
            new Fila("101", "notebook", "lenovo", "l14", "intel i5", "notebooks", "Cat Import")
        };
        var datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.IsTrue(_gestorItems.ExisteItemConEseId(101));
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_LimpiaListasDelLector()
    {
        List<string> titulos = new() { "id", "titulo" };
        List<Fila> filas = new() {
            new Fila("1", "t", "m", "x", "d", "c", "Cat")
        };
        var datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.AreEqual(0, _gestorLectorCsv.Titulos.Count);
        Assert.AreEqual(0, _gestorLectorCsv.Filas.Count);
    }

    [TestMethod]
    public void ImportarItemsDesdeCsv_ReseteaCantidadDeFilas()
    {
        List<string> titulos = new() { "id", "titulo" };
        List<Fila> filas = new() {
            new Fila("1", "t", "m", "x", "d", "c", "Cat")
        };
        var datos = new DatosImportarCsv(titulos, 1, filas);

        _controladorLectorCsv.ImportarItemsDesdeCsv(datos);

        Assert.AreEqual(0, _gestorLectorCsv.CantidadDeFilas);
    }
}