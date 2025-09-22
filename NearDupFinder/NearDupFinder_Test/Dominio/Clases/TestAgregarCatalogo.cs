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

        var ex = Assert.ThrowsException<ArgumentException>(() => s.AgregarCatalogo(null));
        
        Assert.AreEqual( "El catálogo no puede ser null", ex.Message);
    }
    
    [TestMethod]
    public void AgregarCatalogo_Duplicado_Falla()
    {
        var s = new Sistema();
        var c1 = new Catalogo("Stock Tata");
        var c2 = new Catalogo("Stock Tata");

        s.AgregarCatalogo(c1);

        var ex = Assert.ThrowsException<InvalidOperationException>(() => s.AgregarCatalogo(c2));
        StringAssert.Contains(ex.Message, "Ya existe un catálogo con ese título");
    }
}