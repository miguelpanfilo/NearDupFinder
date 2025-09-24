using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Struct;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class TokenizacionPruebas
{
    private Sistema _sistema = null!;
    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
    }
    [TestCleanup]
    public void TearDown()
    {
        _sistema = null!; 
    }
    
    [TestMethod]
    public void CrearToken_Ok()
    {
        Item item = new Item("Iphone 17", "Celular de última generación");
        
        ItemTokenizado tokens = _sistema.TokenizarItem(item);
        
        CollectionAssert.AreEqual(
            new[] { "Iphone", "17" },
            tokens.TokenTitulo
        );

        CollectionAssert.AreEqual(
            new[] { "Celular", "de", "última", "generación" },
            tokens.TokenDescripcion
        );
    }
    
    [TestMethod]
    public void CrearToken_TituloConEspaciosDeMas_Ok()
    {
        Item item = new Item("Iphone   17", "Celular de última generación");
        
        ItemTokenizado tokens = _sistema.TokenizarItem(item);
        
        CollectionAssert.AreEqual(
            new[] { "Iphone", "17" },
            tokens.TokenTitulo
        );

        CollectionAssert.AreEqual(
            new[] { "Celular", "de", "última", "generación" },
            tokens.TokenDescripcion
        );
    }
    [TestMethod]
    public void CrearToken_TextoConNumeros_Ok()
    {

        var item = new Item("ps5 slim 1tb", "ssd 512");

        var tokens = _sistema.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "ps5", "slim", "1tb" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "ssd", "512" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void CrearToken_ConEspaciosExtremos_Ok()
    {
        var item = new Item("   iphone 17   ", "   celular   de   ultima   generacion   ");

        var tokens = _sistema.TokenizarItem(item);

        CollectionAssert.AreEqual(new[] { "iphone", "17" }, tokens.TokenTitulo);
        CollectionAssert.AreEqual(new[] { "celular", "de", "ultima", "generacion" }, tokens.TokenDescripcion);
    }
    
    [TestMethod]
    public void TokenizarItem_ItemNull_TiraArgumentNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _sistema.TokenizarItem(null));
    }
    
}