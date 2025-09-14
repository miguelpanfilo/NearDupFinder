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
    
    [TestMethod]
    public void CrearCatalogo_ErrorSinTituloTest()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => new Catalogo(""));
        
        Assert.AreEqual("El titulo es obligatorio", ex.Message);
    }

    [TestMethod]
    public void CambiarTitulo_OkTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
        c.CambiarTitulo("Nuevo nombre");
        Assert.AreEqual("Nuevo nombre", c.Titulo);
    }
    
    [TestMethod]
    public void CambiarDescripcion_OkTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
        c.CambiarDescripcion("Local numero 145, del barrio Centro");
        Assert.AreEqual("Local numero 145, del barrio Centro", c.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_NullTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
        c.CambiarDescripcion(null);
        Assert.AreEqual("", c.Descripcion);
    }
    
}
