using NearDupFinder_LogicaDeNegocio;
using NearDupFInder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios;

[TestClass]
public class LectorCsvPruebas
{
    [TestMethod]
    public void Constructor_InicializaColeccionesVacias()
    {
        Sistema sistema = new Sistema();
        LectorCsv lector = new LectorCsv(sistema);
        Assert.IsNotNull(lector.Titulos);
        Assert.IsNotNull(lector.Filas);
        Assert.AreEqual(0, lector.Titulos.Count);
        Assert.AreEqual(0, lector.Filas.Count);
        Assert.AreEqual(0, lector.CantidadDeFilas);
    }

    [TestMethod]
    public void LeerCsv_CargaTitulosCantidadYFilas()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        var titulos = new List<string> { "id", "titulo" };
        var filas = new List<Fila> { new Fila("1","t","m","x","d","c","Cat 1") };

        lector.LeerCsv(titulos, 1, filas);

        CollectionAssert.AreEqual(titulos, lector.Titulos);
        Assert.AreEqual(1, lector.CantidadDeFilas);
        Assert.AreEqual(1, lector.Filas.Count);
        Assert.AreEqual("1", lector.Filas[0].Id);
    }

    [TestMethod]
    public void Limpiar_ReiniciaEstado()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id" }, 1, new List<Fila> { new Fila("1","t","m","x","d","c","Cat") });

        lector.Limpiar();

        Assert.AreEqual(0, lector.Titulos.Count);
        Assert.AreEqual(0, lector.Filas.Count);
        Assert.AreEqual(0, lector.CantidadDeFilas);
    }

    [TestMethod]
    public void ImportarItems_CatalogoVacio_SaltaFila()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("10","t","m","x","d","c","") });

        lector.ImportarItems();

        Assert.IsNull(sistema.ObtenerCatalogoPorTitulo(""));
        Assert.IsFalse(sistema.IdExisteEnListaDeIdGlobal(10));
    }

    [TestMethod]
    public void ImportarItems_CreaCatalogoSiNoExiste()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("1","t","m","x","d","c","Cat Nuevo") });

        lector.ImportarItems();

        Assert.IsNotNull(sistema.ObtenerCatalogoPorTitulo("Cat Nuevo"));
    }

    [TestMethod]
    public void ImportarItems_RespetaIdNuevo()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("42","t","m","x","d","c","Cat A") });

        lector.ImportarItems();

        Assert.IsTrue(sistema.IdExisteEnListaDeIdGlobal(42));
    }

    [TestMethod]
    public void ImportarItems_SaltaFilaConIdDuplicado()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("55","t1","m","x","d","c","Cat X") });
        lector.ImportarItems();
        
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("55","t2","m","x","d","c","Cat Y") });
        lector.ImportarItems();

        bool hayDosItems = sistema.CantidadDeItemsGlobal() == 2;
        
        Assert.IsTrue(sistema.IdExisteEnListaDeIdGlobal(55));
        Assert.IsFalse(hayDosItems);
    }

    [TestMethod]
    public void ImportarItems_IdVacio_NoRegistraIdCeroYSeCreaCatalogo()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("","t","m","x","d","c","Cat Z") });

        lector.ImportarItems();
        bool hayAlMenosUnItem = sistema.CantidadDeItemsGlobal() >= 1;

        Assert.IsNotNull(sistema.ObtenerCatalogoPorTitulo("Cat Z"));
        Assert.IsFalse(sistema.IdExisteEnListaDeIdGlobal(0));
        Assert.IsTrue(hayAlMenosUnItem);
    }

    [TestMethod]
    public void ImportarItems_IdCero_SeConsideraInvalido_NoRegistraIdCero()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("0","t","m","x","d","c","Cat W") });

        lector.ImportarItems();

        Assert.IsNotNull(sistema.ObtenerCatalogoPorTitulo("Cat W"));
        Assert.IsFalse(sistema.IdExisteEnListaDeIdGlobal(0));
    }

    [TestMethod]
    public void ImportarItems_ItemInvalido_NoRegistraId_PuedeCrearCatalogo()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        lector.LeerCsv(new List<string> { "id","titulo" }, 1,
            new List<Fila> { new Fila("1","", "m","x","", "c","Cat Invalido") });

        lector.ImportarItems();

        Assert.IsNotNull(sistema.ObtenerCatalogoPorTitulo("Cat Invalido"));
        Assert.IsFalse(sistema.IdExisteEnListaDeIdGlobal(1));
    }
    
    [TestMethod]
    public void ImportarItems_CatalogoInvalido_MasDe120Caracteres_SaltaFilaSinRegistrarIdNiCatalogo()
    {
        var sistema = new Sistema();
        var lector = new LectorCsv(sistema);
        var nombreLargo = new string('X', 121);
        lector.LeerCsv(new List<string> { "id", "titulo" }, 1,
            new List<Fila> { new Fila("123", "t", "m", "x", "d", "c", nombreLargo) });

        lector.ImportarItems();

        Assert.IsNull(sistema.ObtenerCatalogoPorTitulo(nombreLargo));
        Assert.IsFalse(sistema.IdExisteEnListaDeIdGlobal(123));
    }
}
