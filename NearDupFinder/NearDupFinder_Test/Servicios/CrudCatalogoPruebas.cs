using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio;

namespace NearDupFinder_Test.Servicios;

[TestClass]
public class CrudCatalogoPruebas
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
        CollectionAssert.Contains(_sistema.Catalogos.ToList(), c);
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
    
    [TestMethod]
    public void EliminarCatalogo_NoExisteCatalogo_Falla()
    {
        var c = new Catalogo("Stock Tata");
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _sistema.EliminarCatalogo(c));
        StringAssert.Contains(ex.Message,"No existe un catálogo con ese título");
    }
    
    [TestMethod]
    public void EliminarCatalogo_CaseInsensitive_Ok()
    {
        _sistema.AgregarCatalogo(new Catalogo("Stock Tata"));
        _sistema.EliminarCatalogo(new Catalogo("stock tata")); 
        Assert.AreEqual(0, _sistema.CantidadDeCatalogos());
    }
    
    [TestMethod]
    public void EliminarCatalogo_DobleEliminacion_Falla()
    {
        var c = new Catalogo("Catálogo");
        _sistema.AgregarCatalogo(c);
        _sistema.EliminarCatalogo(c);

        var ex = Assert.ThrowsException<InvalidOperationException>(() => _sistema.EliminarCatalogo(c));
        StringAssert.Contains(ex.Message, "No existe un catálogo con ese título");
    }
    
    [TestMethod]
    public void CambiarTituloCatalogo_Ok()
    {
        var c = new Catalogo("Original");
        _sistema.AgregarCatalogo(c);
        
        _sistema.CambiarTituloCatalogo(c, "NuevoTitulo");
        
        Assert.AreEqual("NuevoTitulo", c.Titulo);
    }
    
    [TestMethod]
    public void CambiarTituloCatalogo_TituloYaExiste_Falla()
    {
        // Arrange
        var c1 = new Catalogo("Cat1");
        var c2 = new Catalogo("Cat2");
        _sistema.AgregarCatalogo(c1);
        _sistema.AgregarCatalogo(c2);

        // Act & Assert
        var ex = Assert.ThrowsException<InvalidOperationException>(
            () => _sistema.CambiarTituloCatalogo(c1, "Cat2")
        );
        StringAssert.Contains(ex.Message, "El Título del catálogo ya existe");
    }
    
    [TestMethod]
    public void CambiarTituloCatalogo_TituloMismoCatalogo_NoFalla()
    {
        
        var c = new Catalogo("Cat1");
        _sistema.AgregarCatalogo(c);

        _sistema.CambiarTituloCatalogo(c, "Cat1"); 
        
        Assert.AreEqual("Cat1", c.Titulo); 
    }
}