using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test;

[TestClass]
public class CatalogoTest
{
    
    private Catalogo _catalogo;

    [TestInitialize]
    public void Setup()
    {
        _catalogo = new Catalogo("Stock Tata");
    }
    
    [TestCleanup]
    public void TearDown()
    {
        _catalogo = null;
    }
    
    [TestMethod]
    public void CrearCatalogo_OkTest()
    {
        
        Assert.AreEqual("Stock Tata", _catalogo.Titulo);
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
    public void CrearCatalogo_TituloSoloEspacios_ErrorTest()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => new Catalogo("   "));
        Assert.AreEqual("El titulo es obligatorio", ex.Message);
    }
    
    [TestMethod]
    public void CambiarTitulo_OkTest()
    {
        _catalogo.CambiarTitulo("Nuevo nombre");
        Assert.AreEqual("Nuevo nombre", _catalogo.Titulo);
    }
    
    [TestMethod]
    public void CambiarTitulo_Trim_OK()
    {
        _catalogo.CambiarTitulo("  Hola  ");
        Assert.AreEqual("Hola", _catalogo.Titulo);
    }

    [TestMethod]
    public void CambiarTitulo_ErrorSinTituloTest()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => _catalogo.CambiarTitulo(""));
        Assert.AreEqual("El titulo es obligatorio", ex.Message);
    }
    
    [TestMethod]
    public void CambiarDescripcion_OkTest()
    {
        
        _catalogo.CambiarDescripcion("Local numero 145, del barrio Centro");
        Assert.AreEqual("Local numero 145, del barrio Centro", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Trim_OK()
    {
        _catalogo.CambiarDescripcion("  Centro  ");
        Assert.AreEqual("Centro", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_NullTest()
    {
        _catalogo.CambiarDescripcion(null);
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Vacia_OK()
    {
        _catalogo.CambiarDescripcion("");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_SoloEspacios_OK()
    {
        _catalogo.CambiarDescripcion("   ");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Minimo_OK()
    {
        string d = new string('a', 1);
        _catalogo.CambiarDescripcion(d);
        Assert.AreEqual(1, _catalogo.Descripcion.Length);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Maximo_OK()
    {
        string d = new string('a', 400);
        _catalogo.CambiarDescripcion(d);
        Assert.AreEqual(400, _catalogo.Descripcion.Length);
    }

    [TestMethod]
    public void CambiarDescripcion_MuyLargo_Error()
    {
        string d = new string('a', 401);
        var ex = Assert.ThrowsException<ArgumentException>(() => _catalogo.CambiarDescripcion(d));
        Assert.AreEqual("La descripcion debe tener entre 1 y 400 caracteres", ex.Message);
    }
    
}
