using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Controladores;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class TokenizacionPruebas
{
    private static Item CrearNuevoItem(string titulo, string descripcion) =>
        new Item(titulo, descripcion);
    
    [TestMethod]
    public void Tokenizar_TokensDeUnCaracter_SeDescartan()
    {
        var gestor = new GestorDuplicados();
        var item = CrearNuevoItem("a b c de f 1 2", "x y z 12 q r s");

        var tokens = gestor.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "de" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "12" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void Tokenizar_TituloYDescripcion_Basico()
    {
        var gestor = new GestorDuplicados();
        var item = CrearNuevoItem("Iphone 17", "Celular de última generación");

        var tokens = gestor.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "Iphone", "17" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "Celular", "de", "última", "generación" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void Tokenizar_TituloConMultiplesEspacios_ColapsaSeparadores()
    {
        var gestor = new GestorDuplicados();
        var item = CrearNuevoItem("Iphone   17", "Celular de última generación");

        var tokens = gestor.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "Iphone", "17" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "Celular", "de", "última", "generación" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void Tokenizar_ConEspaciosExtremos_Recorta()
    {
        var gestor = new GestorDuplicados();
        var item = CrearNuevoItem("   iphone 17   ", "   celular   de   ultima   generacion   ");

        var tokens = gestor.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "iphone", "17" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "celular", "de", "ultima", "generacion" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void Tokenizar_TextoConNumeros_MantieneAlfanumericos()
    {
        var gestor = new GestorDuplicados();
        var item = CrearNuevoItem("ps5 slim 1tb", "ssd 512");

        var tokens = gestor.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "ps5", "slim", "1tb" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "ssd", "512" }, tokens.TokenDescripcion);
    }
}