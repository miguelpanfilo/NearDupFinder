using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Struct;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class TokenizacionPruebas
{
    [TestMethod]
    public void CrearToken_Ok()
    {
        Sistema _sistema = new Sistema();
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
        Sistema _sistema = new Sistema();
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
    

}