using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFinder_LogicaDeNegocio.Servicios.Notificaciones;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Importacion;

[TestClass]
public class GestorLectorCsvPruebas
{
    private GestorLectorCsv _gestorLectorCsv = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorItems _gestorItems = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorControlClusters _gestorControlClusters = null!;
    private GestorDuplicados _gestorDuplicados = null!;
    private GestorNotificaciones _gestorNotificaciones = null!;
    private readonly HashSet<int> _idsItemsGlobal = new();
    private ControladorItems _controladorItems = null!;
    private SqlContext _context = null!;
    private IRepositorioDuplicados _repoDuplicados = null!;

    [TestInitialize]
    public void Setup()
    {
        var procesador = new ProcesadorTexto();

        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_LectorCsv");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

        IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);
        IRepositorioItems repoItems = new RepositorioItems(_context);
        IRepositorioClusters repoClusters = new RepositorioClusters(_context);
        IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context);
        IRepositorioNotificaciones repoNotificaciones = new RepositorioNotificaciones(_context);
        _repoDuplicados = new RepositorioDuplicados(_context);

        var sesionUsuario = new SesionUsuarioActual();
        sesionUsuario.Asignar("tester@correo.com");

        _gestorAuditoria  = new GestorAuditoria(repoAuditorias, sesionUsuario);
        _gestorCatalogos  = new GestorCatalogos(repoCatalogos, repoClusters, repoItems);
        _gestorDuplicados = new GestorDuplicados(procesador);
        _gestorNotificaciones = new GestorNotificaciones(repoNotificaciones);
        _gestorControlClusters = new  GestorControlClusters(
            _gestorCatalogos,
            _gestorAuditoria,
            _gestorNotificaciones,
            sesionUsuario,
            repoClusters,
            repoItems
        );

        _idsItemsGlobal.Clear();
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

        Assert.IsTrue(_gestorItems.ExisteItemConEseId(42));
    }

    [TestMethod]
    public void ImportarItems_IdVacio_NoRegistraIdCero()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("","t","m","x","d","c","Cat Z") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsFalse(_gestorItems.ExisteItemConEseId(0));
    }

    [TestMethod]
    public void ImportarItems_ItemInvalido_NoRegistraId_PuedeCrearCatalogo()
    {
        _gestorLectorCsv.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("1","", "m","x","", "c","Cat Invalido") });

        _gestorLectorCsv.ImportarItems();

        Assert.IsNotNull(_gestorCatalogos.ObtenerCatalogoPorTitulo("Cat Invalido"));
        Assert.IsFalse(_gestorItems.ExisteItemConEseId(1));
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
        
        Assert.IsTrue(_gestorItems.ExisteItemConEseId(5));
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
        Assert.IsFalse(_gestorItems.ExisteItemConEseId(123));
    }
}