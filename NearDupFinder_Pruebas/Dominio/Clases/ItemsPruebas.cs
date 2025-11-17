using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Pruebas.Dominio.Clases;

[TestClass]
public class ItemsPruebas
{
    [TestInitialize]
    public void Setup()
    {
        Item.ResetearContadorId();
    }

    [TestMethod]
    public void TestItems_CrearItemTituloLargo_Fallo()
    {
        string tituloLargo = new string('a', 121);
        var error = Assert.ThrowsException<ExcepcionItem>(() =>
            new Item(tituloLargo, "Descripcion válida", 0)
        );
        Assert.AreEqual("El Título no puede superar 120 caracteres.", error.Message);
    }

    [TestMethod]
    public void TestItems_CrearItem_Ok()
    {
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        Assert.AreEqual("Soy un titulo", item.Titulo);
        Assert.AreEqual("Soy una descripcion", item.Descripcion);
        Assert.AreEqual("Marca", item.Marca);
        Assert.AreEqual("Modelo", item.Modelo);
        Assert.AreEqual("Categoria", item.Categoria);
    }

    [TestMethod]
    public void TestItems_CrearItemTitulo_Vacio_Fallo()
    {
        var error = Assert.ThrowsException<ExcepcionItem>(() => new Item("", "Descripcion", 0));
        Assert.AreEqual("El Título es obligatorio", error.Message);
    }

    [TestMethod]
    public void TestItems_CrearItemTituloSoloEspacios_Fallo()
    {
        var error = Assert.ThrowsException<ExcepcionItem>(() => new Item("   ", "Descripcion", 0));
        Assert.AreEqual("El Título es obligatorio", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemDescripcion_Vacio_Fallo()
    { 
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item(
                "Titulo válido",
                "", 
                0,
                "Marca",
                "Modelo",
                "Categoria"
            )
        );
        Assert.AreEqual("La Descripción es obligatoria.", error.Message);
    }

    [TestMethod]
    public void TestItems_CrearItemDescripcion_Solo_Espacios_Fallo()
    { 
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item(
                "Titulo válido",
                "   ", 
                0,
                "Marca",
                "Modelo",
                "Categoria"
            )
        );
        Assert.AreEqual("La Descripción es obligatoria.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemDescripcion_Largo_Fallo()
    {
        string descripcion = new string('A', 401);
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item("Titulo", descripcion, 0) 
        );
        Assert.AreEqual("La descripcion no puede superar 400 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemMarcaLargo_Fallo()
    {
        string marca = new string('A', 61);
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item("Titulo", "descripcion", 0, marca) 
        );
        Assert.AreEqual("La marca no puede superar 60 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemModeloLargo_Fallo()
    {
        string modelo = new string('A', 61);
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item("Titulo", "descripcion", 0, null, modelo) 
        );
        Assert.AreEqual("El modelo no puede superar 60 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemCategoriaLargo_Fallo()
    {
        string categoria = new string('A', 41);
        var error = Assert.ThrowsException<ExcepcionItem>(() => 
            new Item("Titulo", "descripcion", 0, null, null, categoria) 
        );
        Assert.AreEqual("La categoria no puede superar 40 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_ModeloMaximo60Caracteres_Ok()
    {
        string modeloMax = new string('D', 60);
        Item item = new Item
        {
            Titulo= "Titulo",
            Descripcion = "Descripcion válida",
            Marca = "Marca",
            Modelo = modeloMax,  
            Categoria = "Categoria"
        };

        Assert.AreEqual(60, item.Modelo.Length);
        Assert.AreEqual(modeloMax, item.Modelo);
    }
    [TestMethod]
    public void TestItems_CategoriaMaxima40Caracteres_Ok()
    {
        string categoriaMax = new string('E', 40);
        Item item = new Item
        {
            Titulo = "Titulo",
            Descripcion = "Descripcion válida",
            Marca = "Marca",
            Modelo = "Modelo",  
            Categoria = categoriaMax
        };
        Assert.AreEqual(40, item.Categoria.Length);
        Assert.AreEqual(categoriaMax, item.Categoria);
    }
    [TestMethod]
    public void TestItems_MarcaMaxima60Caracteres_Ok()
    {
        string marcaMax = new string('M', 60);
        Item item = new Item
        {
            Titulo = "Titulo",
            Descripcion = "Descripcion válida",
            Marca = marcaMax,
            Modelo = "Modelo",  
            Categoria = "Categoria"
        };
        Assert.AreEqual(marcaMax, item.Marca);
    }
    
    [TestMethod]
    public void TestItem_CrearAsignarIdAutoincremental_Ok()
    {
        Item item1 = Item.Crear("Item 1", "Desc 1");
        Item item2 = Item.Crear("Item 2", "Desc 2");

        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreNotEqual(item1.Id, item2.Id);
    }
    
    [TestMethod]
    public void TestItem_ResetearIdContador_Ok()
    {
        Item item1 = Item.Crear("Uno", "Desc" ,"Categoria", "Marca", "Modelo");
        Item item2 = Item.Crear("Dos", "Desc" ,"Categoria", "Marca", "Modelo");
        
        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        
        Item.ResetearContadorId();
        Item item3 = Item.Crear("tres", "Desc" ,"Categoria", "Marca", "Modelo");
        
        Assert.AreEqual(1, item3.Id); 
    }
  
   [TestMethod]
    public void TestItem_DevolverNoNulls()
    {
        Item item = new Item
        {
            Titulo = "Titulo",
            Descripcion = "Descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };
        var id = item.Id;
        var titulo = item.Titulo;
        var descripcion = item.Descripcion;
        var marca = item.Marca;
        var modelo = item.Modelo;
        var categoria = item.Categoria;
        Assert.IsNotNull(id);
        Assert.IsNotNull(titulo);
        Assert.IsNotNull(descripcion);
        Assert.IsNotNull(marca);
        Assert.IsNotNull(modelo);
        Assert.IsNotNull(categoria);
    }
    [TestMethod]
    public void TestItems_MarcaNull_Ok()
    {
        Item item = new Item { Titulo = "Titulo", Descripcion = "Descripcion", Marca = null };
        Assert.IsNull(item.Marca);
    }

    [TestMethod]
    public void TestItems_ModeloNull_Ok()
    {
        Item item = new Item { Titulo = "Titulo", Descripcion = "Descripcion", Modelo = null };
        Assert.IsNull(item.Modelo);
    }

    [TestMethod]
    public void TestItems_CategoriaNull_Ok()
    {
        Item item = new Item { Titulo = "Titulo", Descripcion = "Descripcion", Categoria = null };
        Assert.IsNull(item.Categoria);
    }
    [TestMethod]
    public void TestItem_EditarTitulo_Fallo()
    {
        Item item = new Item("Titulo original", "Descripcion original", 0);
        item.EditarTitulo("TituloValido");
        Assert.AreEqual("TituloValido", item.Titulo);
        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.EditarTitulo(null);
        });

        Assert.AreEqual("El Título es obligatorio", error.Message);
    }

    [TestMethod]
    public void TestItem_EditarDescripcion_Fallo()
    {
        Item item = new Item("Titulo original", "Descripcion original", 0);
        item.EditarDescripcion("DescripcionValida");
        Assert.AreEqual("DescripcionValida", item.Descripcion);
        ExcepcionItem exception = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.EditarDescripcion("");
        });
        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }

    [TestMethod]
    public void TestItem_EditarMarca_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarMarca("MarcaValida");
        Assert.AreEqual("MarcaValida", item.Marca);
        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.EditarMarca(new string('A', 61));
        });
        Assert.AreEqual("La marca no puede superar 60 caracteres.", error.Message);
    }
    [TestMethod]
    public void TestItem_EditarModelo_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarModelo("ModeloValido");
        Assert.AreEqual("ModeloValido", item.Modelo);
        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.EditarModelo(new string('B', 61));
        });
        Assert.AreEqual("El modelo no puede superar 60 caracteres.", error.Message);
    }

    [TestMethod]
    public void TestItem_EditarCategoria_Fallo()
    { 
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarCategoria("CategoriaValida");
        Assert.AreEqual("CategoriaValida", item.Categoria);
        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.EditarCategoria(new string('C', 41));
        });
        Assert.AreEqual("La categoria no puede superar 40 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItem_EditarTitulo_Ok()
    {
        Item item = new Item("Titulo original", "Descripcion original", 0);
        item.EditarTitulo("Nuevo Titulo");
        Assert.AreEqual("Nuevo Titulo", item.Titulo);
    }

    [TestMethod]
    public void TestItem_EditarDescripcion_Ok()
    {
        Item item = new Item("Titulo", "Descripcion original", 0);
        item.EditarDescripcion("Nueva Descripcion");
        Assert.AreEqual("Nueva Descripcion", item.Descripcion);
    }

    [TestMethod]
    public void TestItem_EditarMarca_Ok()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarMarca("NuevaMarca");
        Assert.AreEqual("NuevaMarca", item.Marca);
    }

    [TestMethod]
    public void TestItem_EditarModelo_Ok()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarModelo("NuevoModelo");
        Assert.AreEqual("NuevoModelo", item.Modelo);
    }

    [TestMethod]
    public void TestItem_EditarCategoria_Ok()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        item.EditarCategoria("NuevaCategoria");
        Assert.AreEqual("NuevaCategoria", item.Categoria);
    }

    [TestMethod]
    public void TestItem_ModificarId_Ok()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        int idViejo = item.Id;
        int idNuevo = idViejo + 100;
        item.AjustarId(idNuevo);
        Assert.AreNotEqual(idViejo, item.Id);
        Assert.AreEqual(idNuevo, item.Id);
    }
    
    [TestMethod]
    public void TestItem_ModificarIdCero_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        int idNuevo = 0;
        ExcepcionItem error = Assert.ThrowsException<ExcepcionItem>(() =>
        {
            item.AjustarId(idNuevo);
        });
        Assert.AreEqual("El id no es valido", error.Message);
    }
    [TestMethod]
    public void TestItem_Equals_DistintoId_RetornaFalse()
    {
        Item item1 = Item.Crear("Item 1", "Desc 1");
        Item item2 = Item.Crear("Item 2", "Desc 2");
        
        Assert.IsFalse(item1.Equals(item2));
    }
    [TestMethod]
    public void TestItem_Equals_Null_RetornaFalse()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        Assert.IsFalse(item.Equals(null));
    }
    [TestMethod]
    public void TestItem_Equals_OtroTipo_RetornaFalse()
    {
        Item item = new Item("Titulo", "Descripcion", 0);
        object obj = new object();
        Assert.IsFalse(item.Equals(obj));
    }
    
    [TestMethod]
    public void TestItem_CrearConStock_StockSeAsignaCorrectamente()
    {
        Item item = new Item("Titulo", "Descripcion", 50);

        Assert.AreEqual(50, item.Stock);
    }
    
    [TestMethod]
    public void Crear_ItemConPrecioEImagen_SeteaCamposCorrectamente()
    {
        string titulo = "Item 1";
        string descripcion = "Descripcion";
        int stock = 5;
        int precio = 100;
        string base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 });

        var item = Item.Crear(
            titulo: titulo,
            descripcion: descripcion,
            marca: "MarcaX",
            modelo: "ModeloY",
            categoria: "CategoriaZ",
            stock: stock,
            precio: precio,
            imagenBase64: base64
        );

        Assert.AreEqual(titulo, item.Titulo);
        Assert.AreEqual(descripcion, item.Descripcion);
        Assert.AreEqual(stock, item.Stock);
        Assert.AreEqual(precio, item.Precio);
        Assert.AreEqual(base64, item.ImagenBase64);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void EditarPrecio_Negativo_LanzaExcepcion()
    {
        var item = Item.Crear("Item", "Desc");
        item.EditarPrecio(-10);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void EditarStock_Negativo_LanzaExcepcion()
    {
        var item = Item.Crear("Item", "Desc");
        item.EditarStock(-1);
    }

    [TestMethod]
    public void EditarImagen_Null_BorraImagen()
    {
        string base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        var item = Item.Crear("Item", "Desc", imagenBase64: base64);

        item.EditarImagen(null);

        Assert.IsNull(item.ImagenBase64);
    }

    [TestMethod]
    [ExpectedException(typeof(ExcepcionItem))]
    public void EditarImagen_Base64Invalido_LanzaExcepcion()
    {
        var item = Item.Crear("Item", "Desc");
        item.EditarImagen("no-es-base64");
    }
}