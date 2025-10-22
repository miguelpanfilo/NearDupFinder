using NearDupFinder_Dominio.Clases;


namespace NearDupFinder_Pruebas.Dominio.Clases;
/*
[TestClass]
public class CatalogoTest
{/*
    
    private Catalogo _catalogo = null!;
    private Sistema _sistema = null!;

    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _catalogo = new Catalogo("Stock Tata");
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
        var largoEsperado = 1;
        
        Assert.AreEqual(largoEsperado, c.Titulo.Length);
    }
    
    [TestMethod]
    public void CrearCatalogo_TituloMaximo_OkTest()
    {
        string titulo = new string('a', 120);
        Catalogo c = new Catalogo(titulo);
        var largoEsperado = 120;
        
        Assert.AreEqual(largoEsperado, c.Titulo.Length);
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
    public void CambiarTitulo_Trim_OkTest()
    {
        _catalogo.CambiarTitulo("  Hola  ");
        Assert.AreEqual("Hola", _catalogo.Titulo);
    }

    [TestMethod]
    public void CambiarTitulo_SinTitulo_ErrorTest()
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
    public void CambiarDescripcion_Trim_OkTest()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"  Centro  ");
        Assert.AreEqual("Centro", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_NullTest_OkTest()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,null);
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Vacia_OkTest()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_SoloEspacios_OkTest()
    {
        _sistema.CambiarDescripcionCatalogo(_catalogo,"   ");
        Assert.AreEqual("", _catalogo.Descripcion);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Minimo_OkTest()
    {
        string d = new string('a', 1);
        
        _sistema.CambiarDescripcionCatalogo(_catalogo,d);
        
        var largoEsperadoDescripcion = 1;
        
        Assert.AreEqual(largoEsperadoDescripcion, _catalogo.Descripcion.Length);
    }
    
    [TestMethod]
    public void CambiarDescripcion_Maximo_OkTest()
    {
        string d = new string('a', 400);
        
        _sistema.CambiarDescripcionCatalogo(_catalogo,d);
        
        var largoEsperadoDescripcion = 400;
        
        Assert.AreEqual(largoEsperadoDescripcion, _catalogo.Descripcion.Length);
    }

    [TestMethod]
    public void CambiarDescripcion_MuyLargo_ErrorTest()
    {
        string d = new string('a', 401);
        
        var ex = Assert.ThrowsException<ArgumentException>(() => _sistema.CambiarDescripcionCatalogo(_catalogo,d));
        
        Assert.AreEqual("La descripcion debe tener entre 1 y 400 caracteres", ex.Message);
    }

    [TestMethod]
    public void AgregarItemACatalogo_OkTest()
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
        
        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.CantidadItems());
        CollectionAssert.Contains(_catalogo.Items.ToList(), item);
    }
    [TestMethod]
    public void AgregarItem_Null_ErrorTest()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(()=> _catalogo.AgregarItem(null!));
        
        Assert.AreEqual("item", ex.ParamName);
        StringAssert.Contains(ex.Message,"El parametro no puede ser Null");
    }

    [TestMethod]
    public void AgregarItem_MismaReferencia_ErrorTest()
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
        
        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.CantidadItems());
    }
    
    [TestMethod]
    public void EliminarItemDeCatalogo_OkTest()
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
        
        var cantidadEsperada = 0;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.CantidadItems());
    }

    [TestMethod]
    public void EliminarItemDeCatalogo_Null_ErrorTest()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(()=> _catalogo.EliminarItem(null!));
        
        Assert.AreEqual("item", ex.ParamName);
        StringAssert.Contains(ex.Message, "El parametro no puede ser Null");
    }
    [TestMethod] 
    public void EliminarItemDeCatalogo_ItemNoExiste_ErrorTest()
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
    
    [TestMethod]
    public void Items_EsSoloLectura_OkTest()
    {
        var items = _catalogo.Items;
        Assert.IsInstanceOfType(items, typeof(IReadOnlyCollection<Item>));
    }
    
    [TestMethod]
    public void CatalogosConIgualTitulo_SonIguales_PeroHashDifiere_OkTest()
    {
        var c1 = new Catalogo("Stock");
        var c2 = new Catalogo("stock");
        
        Assert.AreEqual(c1, c2);

        Assert.AreNotEqual(c1.GetHashCode(), c2.GetHashCode());
    }

    [TestMethod]
    public void CatalogosConTituloDistinto_NoSonIguales_OkTest()
    {
        var c1 = new Catalogo("Stock Tata");
        var c2 = new Catalogo("Stock Disco");

        Assert.AreNotEqual(c1, c2);
    }

    [TestMethod]
    public void Catalogo_ComparadoConOtroTipo_NoEsIgual_OkTest()
    {
        var c1 = new Catalogo("Stock");
        object  noCatalogo = "Stock";

        Assert.IsFalse(c1.Equals(noCatalogo));
    }
    [TestMethod]
    public void Equals_OtroCatalogoNull_DevuelveFalse_OkTest()
    {
        var catalogo = new Catalogo("Prueba");

        bool resultado = catalogo.Equals(null);

        Assert.IsFalse(resultado);
    }
    */
