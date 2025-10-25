using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[TestClass]
public class JaccardPruebas
{
    private GestorDuplicados _gestorDuplicados = null!;
    
    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria) =>
        new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };

    private static Catalogo CrearCatalogo(params Item[] items)
    {
        var catalogo = new Catalogo("Catalogo");
        foreach (var item in items) catalogo.AgregarItem(item);
            return catalogo;
    }
    
    [TestInitialize]
    public void Setup()
    {
        _gestorDuplicados = new GestorDuplicados();
    }

    [TestMethod]
    public void CalcularJaccard_AmbosVacios_DevuelveCero()
    {
        Item itemA = CrearItem("a b c", "desc igual", "m", "x", "cat");
        Item itemB = CrearItem("x y z", "desc igual", "m", "x","cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(0, listaParesDuplicados.Count);
    }

    [TestMethod]
    public void CalcularJaccard_UnoVacio_DevuelveCero()
    {
        Item itemA = CrearItem("a b c", "desc igual", "m", "x","cat");
        Item itemB = CrearItem("de", "desc igual", "m", "x","cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(0, listaParesDuplicados.Count);
    }

    [TestMethod]
    public void CalcularJaccard_AmbosNoVacios_RetornaValorEsperado()
    {
        GestorDuplicados gestor = new GestorDuplicados();
        Item itemA = CrearItem("ab", "desc igual", "m", "x","cat");
        Item itemB = CrearItem("ab cd", "desc igual", "m", "x","cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = gestor.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(1, listaParesDuplicados.Count);
        Assert.AreEqual(0.5f, listaParesDuplicados[0].JaccardTitulo, 1e-6);
    }
}
