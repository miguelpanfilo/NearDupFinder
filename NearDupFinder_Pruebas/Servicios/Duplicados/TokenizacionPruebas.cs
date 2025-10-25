using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;


[TestClass]
public class TokenizacionPruebas
{
   private GestorDuplicados _gestorDuplicados = null!;

    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria) =>
        new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };

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
    public void Tokenizar_TokensDeUnCaracter_SeDescartan()
    {
        Item itemA = CrearItem("a b c de f 1 2", "x y z 12 q r s", "m", "x", "cat");
        Item itemB = CrearItem("a b c de f 1 2", "x y z 12 q r s", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "de" }, listaParesDuplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "12" }, listaParesDuplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Tokenizar_TituloYDescripcion_Basico()
    {
        Item itemA = CrearItem("Iphone 17", "Celular de última generación", "m", "x", "cat");
        Item itemB = CrearItem("Iphone 17", "Celular de última generación", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "iphone", "17" }, listaParesDuplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "celular", "de", "ultima", "generacion" }, listaParesDuplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Tokenizar_TituloConMultiplesEspacios_ColapsaSeparadores()
    {
        Item itemA = CrearItem("Iphone   17", "Celular de última generación", "m", "x", "cat");
        Item itemB = CrearItem("Iphone 17",   "Celular de última generación", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "iphone", "17" }, listaParesDuplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "celular", "de", "ultima", "generacion" }, listaParesDuplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Tokenizar_ConEspaciosExtremos_Recorta()
    {
        Item itemA = CrearItem("   iphone 17   ", "   celular   de   ultima   generacion   ", "m", "x", "cat");
        Item itemB = CrearItem("iphone 17", "celular de ultima generacion", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "iphone", "17" }, listaParesDuplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "celular", "de", "ultima", "generacion" }, listaParesDuplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Tokenizar_TextoConNumeros_MantieneAlfanumericos()
    {
        Item itemA = CrearItem("ps5 slim 1tb", "ssd 512", "m", "x", "cat");
        Item itemB = CrearItem("ps5 slim 1tb", "ssd 512", "m", "x", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> listaParesDuplicados = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "ps5", "slim", "1tb" }, listaParesDuplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "ssd", "512" }, listaParesDuplicados[0].TokensCompartidosDescripcion);
    }
}