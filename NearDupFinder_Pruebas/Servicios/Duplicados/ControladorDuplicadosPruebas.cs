using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[DoNotParallelize]
[TestClass]
public class ControladorDuplicadosPruebas
{
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorDuplicados _gestorDuplicados = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private GestorControlClusters _gestorControlClusters = null!;
    private List<ParDuplicado> _duplicadosGlobales = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    private GestorItems _gestorItems = null!;
    private ControladorItems _controladorItems = null!;
    private SqlContext _context = null!;
    private HashSet<int> _idsItemsGlobal = null!;

    private Item CrearItem(int catalogoId, string titulo, string desc, string marca, string modelo, string categoria)
    {
        var dto = new DatosCrearItem(catalogoId, titulo, desc, categoria, marca, modelo);
        Item item = _controladorItems.CrearItem(dto);
        return item;
    }

    private Catalogo CrearCatalogo(string titulo)
    {
        _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear(titulo));
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo(titulo);
        return catalogo!;
    }

    [TestInitialize]
    public void Setup()
    {
        var procesador = new ProcesadorTexto();

        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Duplicados");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

        IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);
        IRepositorioItems repoItems = new RepositorioItems(_context);
        IRepositorioClusters repoClusters = new RepositorioClusters(_context);
        IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context); // ✅ agregado

        _gestorAuditoria = new GestorAuditoria(repoAuditorias); 
        _gestorDuplicados = new GestorDuplicados(procesador);
        _gestorCatalogos = new GestorCatalogos(repoCatalogos);

        _gestorControlClusters = new GestorControlClusters(
            _gestorCatalogos,
            _gestorAuditoria,
            repoCatalogos,
            repoClusters,
            repoItems
        );

        _idsItemsGlobal = new HashSet<int>();
        _duplicadosGlobales = new List<ParDuplicado>();

        _controladorDuplicados = new ControladorDuplicados(
            _gestorAuditoria,
            _gestorDuplicados,
            _gestorCatalogos,
            _gestorControlClusters,
            _duplicadosGlobales
        );

        _gestorItems = new GestorItems(_idsItemsGlobal, repoItems);

        _controladorItems = new ControladorItems(
            _gestorItems,
            _gestorCatalogos,
            _controladorDuplicados,
            _gestorControlClusters,
            _gestorAuditoria,
            _idsItemsGlobal
        );
    }

    [TestMethod]
    public void ProcesarDuplicadosPorAlta_CatalogoNoExiste_LanzaExcepcionCatalogo()
    {
        int idCatalogo = 999;
        int idItem = 1;

        ExcepcionCatalogo error = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _controladorDuplicados.ProcesarDuplicados(idCatalogo, idItem));
        StringAssert.Contains(error.Message, $"Catálogo no encontrado (Id={idCatalogo}).");
    }

    [TestMethod]
    public void ProcesarDuplicadosPorAlta_ItemNoExiste_LanzaExcepcionItem()
    {
        Catalogo catalogo = CrearCatalogo("Cat");
        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        int idCatalogo = catalogo.Id;
        int idItemInexistente = itemA.Id + 1000;

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.ProcesarDuplicados(idCatalogo, idItemInexistente));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={idItemInexistente}).");
    }

    [TestMethod]
    public void ProcesarDuplicadosPorAlta_DetectaYAgregaADuplicadosGlobales_YSeteaEstadoEnItems()
    {
        Catalogo catalogo = CrearCatalogo("Cat");
        Item itemA = CrearItem(catalogo.Id, "Notebook Lenovo L14", "intel i5 8gb 256gb ssd", "Lenovo", "L14",
            "Notebooks");
        Item itemB = CrearItem(catalogo.Id, "notebook lenovo l14", "intel i5 8gb 256gb ssd", "lenovo", "l14",
            "notebooks");

        Assert.AreEqual(1, _duplicadosGlobales.Count);
        Assert.IsTrue(itemA.EstadoDuplicado);
        Assert.IsTrue(itemB.EstadoDuplicado);
    }

    [TestMethod]
    public void ActualizarDuplicadosPara_CatalogoNoExiste_LanzaExcepcionCatalogo()
    {
        DatosActualizarDuplicados datos = new DatosActualizarDuplicados(123, 1);

        ExcepcionCatalogo error = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _controladorDuplicados.ActualizarDuplicadosPara(datos));
        StringAssert.Contains(error.Message, $"Catálogo no encontrado (Id={datos.IdCatalogo}).");
    }

    [TestMethod]
    public void ActualizarDuplicadosPara_ItemNoExiste_LanzaExcepcionItem()
    {
        Catalogo catalogo = CrearCatalogo("Cat");
        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");

        DatosActualizarDuplicados datos = new DatosActualizarDuplicados(catalogo.Id, itemA.Id + 999);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.ActualizarDuplicadosPara(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItem}).");
    }

    [TestMethod]
    public void ActualizarDuplicadosPara_EliminaPrevios_RecalculaYActualizaEstados()
    {
        Catalogo catalogo = CrearCatalogo("Cat3");

        Item itemA = CrearItem(catalogo.Id, "alpha beta", "lorem ipsum", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "alpha beta", "lorem ipsum", "m", "x", "c");
        Item itemC = CrearItem(catalogo.Id, "gamma delta", "otro texto", "", "", "c");
        
        _duplicadosGlobales.Add(new ParDuplicado(
            itemA, itemB, 0.95f, TipoDuplicado.PosibleDuplicado,
            1.0f, 1.0f, 1, 1,
            new[] { "alpha", "beta" }, new[] { "lorem", "ipsum" },
            catalogo.Id));
        itemA.EstadoDuplicado = true;
        itemB.EstadoDuplicado = true;

        itemA.Titulo = "gamma delta";
        itemA.Descripcion = "otro texto";
        itemA.Marca = "";
        itemA.Modelo = "";

        DatosActualizarDuplicados datos = new DatosActualizarDuplicados(catalogo.Id, itemA.Id);
        
        _controladorDuplicados.ActualizarDuplicadosPara(datos);
            
        Assert.AreEqual(1, _duplicadosGlobales.Count);
        Assert.AreEqual(itemC.Id, _duplicadosGlobales[0].ItemPosibleDuplicado.Id);

        Assert.IsTrue(itemA.EstadoDuplicado);
        Assert.IsFalse(itemB.EstadoDuplicado);
        Assert.IsTrue(itemC.EstadoDuplicado);
    }

    [TestMethod]
    public void ObtenerDuplicadosOrdenados_DevuelveDtoOrdenadoPorScoreDesc()
    {
        Catalogo catalogo = CrearCatalogo("Cat");

        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");
        Item itemC = CrearItem(catalogo.Id, "c", "d", "m", "x", "c");

        _duplicadosGlobales.Add(new ParDuplicado(
            itemA, itemB, 0.70f, TipoDuplicado.AlertaDuplicado,
            0.8f, 0.6f, 1, 0,
            new[] { "a" }, new[] { "d" },
            catalogo.Id));
        _duplicadosGlobales.Add(new ParDuplicado(
            itemA, itemC, 0.90f, TipoDuplicado.PosibleDuplicado,
            1.0f, 0.8f, 1, 1,
            new[] { "a" }, new[] { "d" },
            catalogo.Id));

        List<DatosParDuplicado> listaDtos = _controladorDuplicados.ObtenerDuplicadosOrdenados();

        Assert.AreEqual(2, listaDtos.Count);
        Assert.IsTrue(listaDtos[0].Score >= listaDtos[1].Score);
        Assert.AreEqual(0.90f, listaDtos[0].Score, 1e-6);
        Assert.AreEqual(0.70f, listaDtos[1].Score, 1e-6);
    }

    [TestMethod]
    public void EliminarDuplicadosPrevios_EliminaTodosLosParesQueInvolucranElItem()
    {
        Catalogo catalogo = CrearCatalogo("Cat");

        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");
        Item itemC = CrearItem(catalogo.Id, "c", "d", "m", "x", "c");

        _duplicadosGlobales.Add(new ParDuplicado(itemA, itemB, 0.80f, TipoDuplicado.AlertaDuplicado,
            1.0f, 1.0f, 1, 1, new[] { "t" }, new[] { "d" }, catalogo.Id));
        _duplicadosGlobales.Add(new ParDuplicado(itemC, itemA, 0.90f, TipoDuplicado.PosibleDuplicado,
            1.0f, 1.0f, 1, 1, new[] { "t" }, new[] { "d" }, catalogo.Id));

        _controladorDuplicados.EliminarDuplicadosPrevios(itemA);

        Assert.AreEqual(0, _duplicadosGlobales.Count);
    }

    [TestMethod]
    public void ActualizarEstadoDuplicadosEnCatalogo_SeteaTrueSoloParaItemsEnPares()
    {
        Catalogo catalogo = CrearCatalogo("Cat");

        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");
        Item itemC = CrearItem(catalogo.Id, "c", "d", "m", "x", "c");

        _duplicadosGlobales.Add(new ParDuplicado(itemA, itemB, 0.88f, TipoDuplicado.PosibleDuplicado,
            1.0f, 1.0f, 1, 1, new[] { "t" }, new[] { "d" }, catalogo.Id));

        _controladorDuplicados.ActualizarEstadoDuplicadosEnCatalogo(catalogo);

        Assert.IsTrue(itemA.EstadoDuplicado);
        Assert.IsTrue(itemB.EstadoDuplicado);
        Assert.IsFalse(itemC.EstadoDuplicado);
    }

    [TestMethod]
    public void DescartarParDuplicado_CatalogoNoExiste_LanzaExcepcionCatalogo()
    {
        DatosDuplicados datos = new DatosDuplicados(999, 1, 2);

        ExcepcionCatalogo error = Assert.ThrowsException<ExcepcionCatalogo>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"Catálogo no encontrado (Id={datos.IdCatalogo}).");
    }

    [TestMethod]
    public void DescartarParDuplicado_ItemANoExiste_LanzaExcepcionItem()
    {
        Catalogo catalogo = CrearCatalogo("Cat");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemB.Id + 999, itemB.Id);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItemAComparar}).");
    }

    [TestMethod]
    public void DescartarParDuplicado_ItemBNoExiste_LanzaExcepcionItem()
    {
        Catalogo catalogo = CrearCatalogo("Cat");
        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemA.Id, itemA.Id + 999);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItemPosibleDuplicado}).");
    }

    [TestMethod]
    public void DescartarParDuplicado_ParNoExiste_LanzaExcepcionDuplicado()
    {
        Catalogo catalogo = CrearCatalogo("Cat");

        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemA.Id, itemB.Id);

        ExcepcionDuplicado error = Assert.ThrowsException<ExcepcionDuplicado>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message,
            $"El par (A={itemA.Id}, B={itemB.Id}) no estaba en la lista de duplicados.");
    }

    [TestMethod]
    public void DescartarParDuplicado_EliminaPar_ActualizaEstados()
    {
        Catalogo catalogo = CrearCatalogo("Cat");

        Item itemA = CrearItem(catalogo.Id, "a", "d", "m", "x", "c");
        Item itemB = CrearItem(catalogo.Id, "b", "d", "m", "x", "c");
        Item itemC = CrearItem(catalogo.Id, "c", "d", "m", "x", "c");


        _duplicadosGlobales.Add(new ParDuplicado(itemA, itemB, 0.80f, TipoDuplicado.AlertaDuplicado,
            1.0f, 1.0f, 1, 1, new[] { "t" }, new[] { "d" }, catalogo.Id));
        _duplicadosGlobales.Add(new ParDuplicado(itemA, itemC, 0.90f, TipoDuplicado.PosibleDuplicado,
            1.0f, 1.0f, 1, 1, new[] { "t" }, new[] { "d" }, catalogo.Id));
        itemA.EstadoDuplicado = true;
        itemB.EstadoDuplicado = true;
        itemC.EstadoDuplicado = true;

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemA.Id, itemB.Id);

        _controladorDuplicados.DescartarParDuplicado(datos);

        Assert.AreEqual(1, _duplicadosGlobales.Count);
        Assert.AreEqual(itemC.Id, _duplicadosGlobales[0].ItemPosibleDuplicado.Id);

        Assert.IsTrue(itemA.EstadoDuplicado);
        Assert.IsFalse(itemB.EstadoDuplicado);
        Assert.IsTrue(itemC.EstadoDuplicado);
    }
}