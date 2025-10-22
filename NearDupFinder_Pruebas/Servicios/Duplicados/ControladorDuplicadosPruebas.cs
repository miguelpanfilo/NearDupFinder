using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[TestClass]
public class ControladorDuplicadosPruebas
{
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorDuplicados _gestorDuplicados = null!;
    private GestorCatalogos _gestorCatalogos = null!;
    private AlmacenamientoDeDatos _almacenamiento = null!;
    private List<ParDuplicado> _duplicadosGlobales = null!;
    private ControladorDuplicados _controladorDuplicados = null!;
    
    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria)
    {
        Item item = new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };
        return item;
    }

    private static Catalogo CrearCatalogo(string titulo, params Item[] items)
    {
        Catalogo catalogo = new Catalogo(titulo);
        foreach (Item item in items) catalogo.AgregarItem(item);
        return catalogo;
    }
    
    [TestInitialize]
    public void Setup()
    {
        _gestorAuditoria = new GestorAuditoria();
        _gestorDuplicados = new GestorDuplicados();
        _almacenamiento = new AlmacenamientoDeDatos();
        _gestorCatalogos = new GestorCatalogos(_almacenamiento);
        _duplicadosGlobales = new List<ParDuplicado>();
        _controladorDuplicados = new ControladorDuplicados(_gestorAuditoria, _gestorDuplicados, _gestorCatalogos, _duplicadosGlobales);
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
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA);
        _almacenamiento.AgregarCatalogo(catalogo);

        int idCatalogo = catalogo.Id;
        int idItemInexistente = itemA.Id + 1000;

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.ProcesarDuplicados(idCatalogo, idItemInexistente));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={idItemInexistente}).");
    }

    [TestMethod]
    public void ProcesarDuplicadosPorAlta_DetectaYAgregaADuplicadosGlobales_YSeteaEstadoEnItems()
    {
        Item itemA = CrearItem("Notebook Lenovo L14", "intel i5 8gb 256gb ssd", "Lenovo", "L14", "Notebooks");
        Item itemB = CrearItem("notebook lenovo l14", "intel i5 8gb 256gb ssd", "lenovo", "l14", "notebooks");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB);
        _almacenamiento.AgregarCatalogo(catalogo);

        _controladorDuplicados.ProcesarDuplicados(catalogo.Id, itemA.Id);

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
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA);
        _almacenamiento.AgregarCatalogo(catalogo);

        DatosActualizarDuplicados datos = new DatosActualizarDuplicados(catalogo.Id, itemA.Id + 999);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.ActualizarDuplicadosPara(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItem}).");
    }

    [TestMethod]
    public void ActualizarDuplicadosPara_EliminaPrevios_RecalculaYActualizaEstados()
    {
        Item itemA = CrearItem("alpha beta", "lorem ipsum", "m", "x", "c");
        Item itemB = CrearItem("alpha beta", "lorem ipsum", "m", "x", "c");
        Item itemC = CrearItem("gamma delta", "otro texto", "", "", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB, itemC);
        _almacenamiento.AgregarCatalogo(catalogo);

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
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Item itemC = CrearItem("c", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB, itemC);
        _almacenamiento.AgregarCatalogo(catalogo);

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
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Item itemC = CrearItem("c", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB, itemC);
        _almacenamiento.AgregarCatalogo(catalogo);

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
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Item itemC = CrearItem("c", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB, itemC);
        _almacenamiento.AgregarCatalogo(catalogo);

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
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemB);
        _almacenamiento.AgregarCatalogo(catalogo);

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemB.Id + 999, itemB.Id);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItemAComparar}).");
    }

    [TestMethod]
    public void DescartarParDuplicado_ItemBNoExiste_LanzaExcepcionItem()
    {
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA);
        _almacenamiento.AgregarCatalogo(catalogo);

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemA.Id, itemA.Id + 999);

        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"Ítem no encontrado (Id={datos.IdItemPosibleDuplicado}).");
    }

    [TestMethod]
    public void DescartarParDuplicado_ParNoExiste_LanzaExcepcionDuplicado()
    {
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB);
        _almacenamiento.AgregarCatalogo(catalogo);

        DatosDuplicados datos = new DatosDuplicados(catalogo.Id, itemA.Id, itemB.Id);

        ExcepcionDuplicado error = Assert.ThrowsException<ExcepcionDuplicado>(() =>
            _controladorDuplicados.DescartarParDuplicado(datos));
        StringAssert.Contains(error.Message, $"El par (A={itemA.Id}, B={itemB.Id}) no estaba en la lista de duplicados.");
    }

    [TestMethod]
    public void DescartarParDuplicado_EliminaPar_ActualizaEstados()
    {
        Item itemA = CrearItem("a", "d", "m", "x", "c");
        Item itemB = CrearItem("b", "d", "m", "x", "c");
        Item itemC = CrearItem("c", "d", "m", "x", "c");
        Catalogo catalogo = CrearCatalogo("Cat", itemA, itemB, itemC);
        _almacenamiento.AgregarCatalogo(catalogo);

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
