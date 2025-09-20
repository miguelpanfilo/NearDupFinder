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
    public void CrearCatalogoLargo_ErrorTest()
    {
        string titulo = new string('a', 121);

        var ex = Assert.ThrowsException<ArgumentException>(() => new Catalogo(titulo));
        Assert.AreEqual("El titulo debe tener entre 1 y 120 caracteres", ex.Message);
    }
    
    
    [TestMethod]
    public void CrearCatalogo_ErrorSinTituloTest()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => new Catalogo(""));
        
        Assert.AreEqual("El titulo es obligatorio", ex.Message);
    }
    
    [TestMethod]
    public void CrearCatalogo_TituloMinimo_OkTest()
    {
        string titulo = new string('a', 1); 
        Catalogo c = new Catalogo(titulo);
        Assert.AreEqual(1, c.Titulo.Length);
    }
    
    [TestMethod]
    public void CrearCatalogo_TituloMaximo_OkTest()
    {
        string titulo = new string('a', 120);
        Catalogo c = new Catalogo(titulo);
        Assert.AreEqual(120, c.Titulo.Length);
    }


    [TestMethod]
    public void CambiarTitulo_OkTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
        c.CambiarTitulo("Nuevo nombre");
        Assert.AreEqual("Nuevo nombre", c.Titulo);
    }

    [TestMethod]
    public void CambiarTitulo_ErrorSinTituloTest()
    {
        Catalogo c = new Catalogo("Stock Tata");
        var ex = Assert.ThrowsException<ArgumentException>(() => c.CambiarTitulo(""));
        Assert.AreEqual("El titulo es obligatorio", ex.Message);
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
