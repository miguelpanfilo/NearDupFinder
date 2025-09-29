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
        CollectionAssert.Contains(_sistema.Catalogos.ToList(), c);
    }
    
    [TestMethod]
    public void AgregarCatalogo_Null_Falla()
    {

        var ex = Assert.ThrowsException<ArgumentNullException>(() => _sistema.AgregarCatalogo(null));
        Assert.AreEqual("catalogo", ex.ParamName);
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
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
    public void EliminarCatalogo_Null_Falla()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _sistema.EliminarCatalogo(null));
        Assert.AreEqual("catalogo", ex.ParamName);
        StringAssert.Contains(ex.Message,"El parametro no puede ser null");
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
    public void RegistrarDetecciones_AgregaDuplicadosAlCatalogo()
    {
        var catalogo = new Catalogo("Catalogo Test");
        var item1 = new Item("Item 1", "Descripcion 1");
        var item2 = new Item("Item 2", "Descripcion 2");

        var duplicado = new Duplicados();
        duplicado.ItemsInvolucrados.Add(item1);
        duplicado.ItemsInvolucrados.Add(item2);

        
        catalogo.RegistrarDetecciones(new List<Duplicados> { duplicado });
        var detecciones = catalogo.ObtenerDetecciones();

        Assert.AreEqual(1, detecciones.Count);
        CollectionAssert.Contains(detecciones.ToList(), duplicado);
        Assert.AreEqual(2, detecciones.First().ItemsInvolucrados.Count);
    }
}