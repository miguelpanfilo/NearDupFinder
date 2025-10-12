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
        var ex = Assert.ThrowsException<ItemException>(() =>
            new Item(tituloLargo, "Descripcion válida")
        );
        Assert.AreEqual("El Título no puede superar 120 caracteres.", ex.Message);
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
        var error = Assert.ThrowsException<ItemException>(() => new Item("", "Descripcion"));
        Assert.AreEqual("El Título es obligatorio", error.Message);
    }

    [TestMethod]
    public void TestItems_CrearItemTituloSoloEspacios_Fallo()
    {
        var error = Assert.ThrowsException<ItemException>(() => new Item("   ", "Descripcion"));
        Assert.AreEqual("El Título es obligatorio", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemDescripcion_Vacio_Fallo()
    { 
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item(
                "Titulo válido",
                "",          
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
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item(
                "Titulo válido",
                "   ",       
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
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", descripcion) 
        );
        Assert.AreEqual("La descripcion no puede superar 400 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemMarcaLargo_Fallo()
    {
        string marca = new string('A', 61);
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", marca) 
        );
        Assert.AreEqual("La marca no puede superar 60 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemModeloLargo_Fallo()
    {
        string modelo = new string('A', 61);
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", null, modelo) 
        );
        Assert.AreEqual("El modelo no puede superar 60 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItems_CrearItemCategoriaLargo_Fallo()
    {
        string categoria = new string('A', 41);
        var error = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", null, null, categoria) 
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
        Assert.AreEqual(60, item.Marca.Length);
        Assert.AreEqual(marcaMax, item.Marca);
    }
    
    [TestMethod]
    public void TestItem_CrearAsignarIdAutoincremental_Ok()
    {
        Item item1 = new Item { Titulo = "Item 1", Descripcion = "Desc 1" };
        Item item2 = new Item { Titulo = "Item 2", Descripcion = "Desc 2" };
        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreNotEqual(item1.Id, item2.Id);
    }
    
    [TestMethod]
    public void TestItem_ResetearIdContador_Ok()
    {
        var item1 = new Item { Titulo = "Uno", Descripcion = "Desc" , Categoria = "Categoria",  Marca = "Marca", Modelo = "Modelo" };
        var item2 = new Item { Titulo = "Dos", Descripcion = "Desc",  Categoria = "Categoria", Marca = "Marca", Modelo = "Modelo" };
        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Item.ResetearContadorId();
        var item3 = new Item { Titulo = "Tres", Descripcion = "Desc"};
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
        Item item = new Item("Titulo original", "Descripcion original");
        item.EditarTitulo("TituloValido");
        Assert.AreEqual("TituloValido", item.Titulo);
        ItemException error = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarTitulo(null);
        });

        Assert.AreEqual("El Título es obligatorio", error.Message);
    }

    [TestMethod]
    public void TestItem_EditarDescripcion_Fallo()
    {
        Item item = new Item("Titulo original", "Descripcion original");
        item.EditarDescripcion("DescripcionValida");
        Assert.AreEqual("DescripcionValida", item.Descripcion);
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarDescripcion("");
        });
        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }

    [TestMethod]
    public void TestItem_EditarMarca_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion");
        item.EditarMarca("MarcaValida");
        Assert.AreEqual("MarcaValida", item.Marca);
        ItemException error = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarMarca(new string('A', 61));
        });
        Assert.AreEqual("La marca no puede superar 60 caracteres.", error.Message);
    }
    [TestMethod]
    public void TestItem_EditarModelo_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion");
        item.EditarModelo("ModeloValido");
        Assert.AreEqual("ModeloValido", item.Modelo);
        ItemException error = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarModelo(new string('B', 61));
        });
        Assert.AreEqual("El modelo no puede superar 60 caracteres.", error.Message);
    }

    [TestMethod]
    public void TestItem_EditarCategoria_Fallo()
    { 
        Item item = new Item("Titulo", "Descripcion");
        item.EditarCategoria("CategoriaValida");
        Assert.AreEqual("CategoriaValida", item.Categoria);
        ItemException error = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarCategoria(new string('C', 41));
        });
        Assert.AreEqual("La categoria no puede superar 40 caracteres.", error.Message);
    }
    
    [TestMethod]
    public void TestItem_EditarTitulo_Ok()
    {
        Item item = new Item("Titulo original", "Descripcion original");
        item.EditarTitulo("Nuevo Titulo");
        Assert.AreEqual("Nuevo Titulo", item.Titulo);
    }

    [TestMethod]
    public void TestItem_EditarDescripcion_Ok()
    {
        Item item = new Item("Titulo", "Descripcion original");
        item.EditarDescripcion("Nueva Descripcion");
        Assert.AreEqual("Nueva Descripcion", item.Descripcion);
    }

    [TestMethod]
    public void TestItem_EditarMarca_Ok()
    {
        Item item = new Item("Titulo", "Descripcion");
        item.EditarMarca("NuevaMarca");
        Assert.AreEqual("NuevaMarca", item.Marca);
    }

    [TestMethod]
    public void TestItem_EditarModelo_Ok()
    {
        Item item = new Item("Titulo", "Descripcion");
        item.EditarModelo("NuevoModelo");
        Assert.AreEqual("NuevoModelo", item.Modelo);
    }

    [TestMethod]
    public void TestItem_EditarCategoria_Ok()
    {
        Item item = new Item("Titulo", "Descripcion");
        item.EditarCategoria("NuevaCategoria");
        Assert.AreEqual("NuevaCategoria", item.Categoria);
    }

    [TestMethod]
    public void TestItem_ModificarId_Ok()
    {
        Item item = new Item("Titulo", "Descripcion");
        int idViejo = item.Id;
        int idNuevo = idViejo + 100;
        item.AjustarId(idNuevo);
        Assert.AreNotEqual(idViejo, item.Id);
        Assert.AreEqual(idNuevo, item.Id);
    }
    
    [TestMethod]
    public void TestItem_ModificarIdCero_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion");
        int idNuevo = 0;
        ItemException error = Assert.ThrowsException<ItemException>(() =>
        {
            item.AjustarId(idNuevo);
        });
        Assert.AreEqual("El id no es valido", error.Message);
    }
}