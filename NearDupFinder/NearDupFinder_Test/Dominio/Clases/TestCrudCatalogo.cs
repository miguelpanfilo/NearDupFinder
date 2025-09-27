using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class TestCrudCatalogo
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
    public void AgregarCatalogo_OkTest()
    {
        Catalogo c = new Catalogo("Catálogo");
        
        _sistema.AgregarCatalogo(c);
        
        Assert.AreEqual(1, _sistema.CantidadDeCatalogos());
        
    }
    
    [TestMethod]
    public void AgregarCatalogo_Null_Falla()
    {

        var ex = Assert.ThrowsException<ArgumentNullException>(() => _sistema.AgregarCatalogo(null));
        Assert.AreEqual("c", ex.ParamName);
        StringAssert.Contains(ex.Message, "El catálogo no puede ser null");
    }
    
    [TestMethod]
    public void AgregarCatalogo_Duplicado_Falla()
    {
        var c1 = new Catalogo("Stock Tata");
        var c2 = new Catalogo("Stock Tata");

        _sistema.AgregarCatalogo(c1);

        var ex = Assert.ThrowsException<InvalidOperationException>(() => _sistema.AgregarCatalogo(c2));
        StringAssert.Contains(ex.Message, "Ya existe un catálogo con ese título");
    }
    
    [TestMethod]
    public void AgregarVariosCatalogos_Ok()
    { 
        _sistema.AgregarCatalogo(new Catalogo("Stock Tata"));
        _sistema.AgregarCatalogo(new Catalogo("Ofertas"));

        Assert.AreEqual(2, _sistema.CantidadDeCatalogos());
    }
    
    [TestMethod]
    public void AgregarCatalogo_SeGuardaCorrectamente()
    {
        var c = new Catalogo("Stock Tata");

        _sistema.AgregarCatalogo(c);

        Assert.AreSame(c, _sistema.ObtenerCatalogoPorTitulo(c.Titulo));
    }
    
    [TestMethod]
    public void EliminarCatalogo_OkTest()
    {
        Catalogo c = new Catalogo("Catálogo");
        
        _sistema.AgregarCatalogo(c);
        
        _sistema.EliminarCatalogo(c);
        
        Assert.AreEqual(0, _sistema.CantidadDeCatalogos());
        
    }
}