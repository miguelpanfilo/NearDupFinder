using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class TestAgregarCatalogo
{   
    [TestMethod]
    public void AgregarCatalogo_OkTest()
    {
        Sistema s = new Sistema();
        Catalogo c = new Catalogo("Catálogo");
        
        s.AgregarCatalogo(c);
        
        Assert.AreEqual(1, s.CantidadDeCatalogos());
        
    }
    
    [TestMethod]
    public void AgregarCatalogo_Null_Falla()
    {
        Sistema s = new Sistema();

        var ex = Assert.ThrowsException<ArgumentNullException>(() => s.AgregarCatalogo(null));

        StringAssert.Contains(ex.Message, "El catálogo no puede ser null");
    }
}