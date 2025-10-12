using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio;


namespace NearDupFinder_Test;

[TestClass]
public class CatalogoTest
{
    
    private Catalogo _catalogo;
    private Sistema _sistema;

    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
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
        
        _sistema.CambiarDescripcionCatalogo(_catalogo,"Local numero 145, del barrio Centro");
        Assert.AreEqual("Local numero 145, del barrio Centro", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Trim_OK()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"  Centro  ");
        Assert.AreEqual("Centro", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_NullTest()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,null);
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Vacia_OK()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_SoloEspacios_OK()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"   ");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Minimo_OK()
    {
        string d = new string('a', 1);
        _sistema.CambiarDescripcionCatalogo(_catalogo,d);
        Assert.AreEqual(1, _catalogo.Descripcion.Length);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Maximo_OK()
    {
        string d = new string('a', 400);
        _sistema.CambiarDescripcionCatalogo(_catalogo,d);
        Assert.AreEqual(400, _catalogo.Descripcion.Length);
    }

    [TestMethod]
    public void CambiarDescripcion_MuyLargo_Error()
    {
        string d = new string('a', 401);
        var ex = Assert.ThrowsException<ArgumentException>(() => _sistema.CambiarDescripcionCatalogo(_catalogo,d));
        Assert.AreEqual("La descripcion debe tener entre 1 y 400 caracteres", ex.Message);
    }

    [TestMethod]
    public void agregarItemACatalogo_ok()
    {
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        
        _catalogo.AgregarItem(item);
        
        Assert.AreEqual(1, _catalogo.CantidadItems());
        CollectionAssert.Contains(_catalogo.Items.ToList(), item);
    }
    [TestMethod]
    public void AgregarItem_Null_Falla()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(()=> _catalogo.AgregarItem(null)); ;
        
        Assert.AreEqual("item", ex.ParamName);
        StringAssert.Contains(ex.Message,"El parametro no puede ser Null");
    }

    [TestMethod]
    public void AgregarItem_MismaReferencia_Falla()
    {
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        
        _catalogo.AgregarItem(item);
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.AgregarItem(item));
        
        Assert.AreEqual("El item ya se encuentra en el catálogo", ex.Message);
        Assert.AreEqual(1, _catalogo.CantidadItems());
    }
    
    [TestMethod]
    public void EliminarItemDeCatalogo_ok()
    {
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        
        _catalogo.AgregarItem(item);
        _catalogo.EliminarItem(item);
        
        Assert.AreEqual(0, _catalogo.CantidadItems());
    }

    [TestMethod]
    public void EliminarItemDeCatalogo_Null_Falla()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(()=> _catalogo.EliminarItem(null));
        
        Assert.AreEqual("item", ex.ParamName);
        StringAssert.Contains(ex.Message, "El parametro no puede ser Null");
    }
    [TestMethod] 
    public void EliminarItemDeCatalogo_ItemNoExiste_Falla()
    {
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.EliminarItem(item));

        Assert.AreEqual("El item no se encuentra en el catálogo", ex.Message);
    }
    
    /* Este test chequea que
    Que el catálogo no permite modificar los ítems desde afuera a través de la colección expuesta.
    Que el diseño respeta el principio de encapsulamiento, manteniendo la lista interna _items privada y protegida.
    Que los métodos públicos (AgregarItem, EliminarItem) son la única forma de modificar el contenido del catálogo.*/
    [TestMethod]
    public void Items_EsSoloLectura()
    {
        var ro = _catalogo.Items;
        Assert.IsInstanceOfType(ro, typeof(IReadOnlyCollection<Item>));
    }
    
    [TestMethod]
    public void CatalogosConIgualTitulo_SonIguales()
    {
        var c1 = new Catalogo("Stock");
        var c2 = new Catalogo("stock"); // mismo texto, distinto casing

        Assert.AreEqual(c1, c2); // usa Equals internamente
        Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
    }

    [TestMethod]
    public void CatalogosConTituloDistinto_NoSonIguales()
    {
        var c1 = new Catalogo("Stock Tata");
        var c2 = new Catalogo("Stock Disco");

        Assert.AreNotEqual(c1, c2);
    }

    [TestMethod]
    public void Catalogo_ComparadoConOtroTipo_NoEsIgual()
    {
        var c1 = new Catalogo("Stock");
        var noCatalogo = "Stock";

        Assert.IsFalse(c1.Equals(noCatalogo));
    }
}
