using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[TestClass]
public class ScorePruebas
{
    private GestorDuplicados _gestorDuplicados = null!;

    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria)
    {
        Item item = new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };
        return item;
    }

    private static Catalogo CrearCatalogo(params Item[] items)
    {
        Catalogo catalogo = new Catalogo("Catalogo");
        foreach (Item item in items) catalogo.AgregarItem(item);
            return catalogo;
    }

    [TestInitialize]
    public void Setup()
    {
        _gestorDuplicados = new GestorDuplicados();
    }

    [TestMethod]
    public void CalcularScore_TituloYDescripcionUno_SinMarcaModelo_Retorna0_80()
    {
        Item itemA = CrearItem("n1", "d1", "", "", "cat");
        Item itemB = CrearItem("n1", "d1", "", "", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);
        
        Assert.AreEqual(0.80f, listaParDuplicados[0].Score, 1e-6);
    }

    [TestMethod]
    public void CalcularScore_TodoUno_Retorna1_00()
    {
        Item itemA = CrearItem("n1", "d1", "m", "x", "cat");
        Item itemB = CrearItem("n1", "d1", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(1.0f, listaParDuplicados[0].Score, 1e-6);
    }
}
