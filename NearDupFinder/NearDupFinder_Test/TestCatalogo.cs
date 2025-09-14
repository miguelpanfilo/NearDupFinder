using NearDupFinder_Dominio;

namespace NearDupFinder_Test;

[TestClass]
public class CatalogoTest
{
    [TestMethod]
    public void CrearCatalogo_OkTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
		
        Assert.AreEqual("Stock Tata", c.Titulo);
    }
    
    

}
