using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Test;

[TestClass]
public class ItemsPruebas
{  
    [TestInitialize]
    public void Setup()
    {
        Item.ResetIdCounter();
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Titulo_Largo_Fallo()
    {
        // Título  supera el máximo permitido
        string tituloLargo = new string('a', 121);
    
        var ex = Assert.ThrowsException<ItemException>(() => 
                new Item(tituloLargo, "Descripcion válida") 
        );
    
        Assert.AreEqual("El Título no puede superar 120 caracteres.", ex.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Ok()
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
    public void TestItems_Crear_Item_Titulo_Vacio_Fallo()
    {
        var ex = Assert.ThrowsException<ItemException>(() => new Item("", "Descripcion"));
        Assert.AreEqual("El Título es obligatorio", ex.Message);
    }

    [TestMethod]
    public void TestItems_Crear_Item_Titulo_SoloEspacios_Fallo()
    {
        var ex = Assert.ThrowsException<ItemException>(() => new Item("   ", "Descripcion"));
        Assert.AreEqual("El Título es obligatorio", ex.Message);
    }



    
    [TestMethod]
    public void TestItems_Crear_Item_Descripcion_Vacio_Fallo()
    {
        // Act & Assert: descripción vacía
        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item(
                "Titulo válido",
                "",          // descripción vacía
                "Marca",
                "Modelo",
                "Categoria"
            )
        );

        Assert.AreEqual("La Descripción es obligatoria.", ex.Message);
    }

    [TestMethod]
    public void TestItems_Crear_Item_Descripcion_Solo_Espacios_Fallo()
    {
        // Act & Assert: descripción solo espacios
        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item(
                "Titulo válido",
                "   ",       // descripción solo espacios
                "Marca",
                "Modelo",
                "Categoria"
            )
        );

        Assert.AreEqual("La Descripción es obligatoria.", ex.Message);
    }


    [TestMethod]
    public void TestItems_Crear_Item_Descripcion_Largo_Fallo()
    {
        string descripcion = new string('A', 401);

        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", descripcion) 
        );
    
        Assert.AreEqual("La descripcion no puede superar 400 caracteres.", ex.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Marca_Largo_Fallo()
    {
        string marca = new string('A', 401);

        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", marca) 
        );
    
        Assert.AreEqual("La marca no puede superar 60 caracteres.", ex.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Modelo_Largo_Fallo()
    {
        string modelo = new string('A', 401);

        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", null, modelo) 
        );
    
        Assert.AreEqual("El modelo no puede superar 60 caracteres.", ex.Message);
    }

    

    [TestMethod]
    public void TestItems_Crear_Item_Categoria_Largo_Fallo()
    {
        string categoria = new string('A', 401);

        var ex = Assert.ThrowsException<ItemException>(() => 
            new Item("Titulo", "descripcion", null, null, categoria) 
        );
    
        Assert.AreEqual("La categoria no puede superar 40 caracteres.", ex.Message);
    }
    
    
    [TestMethod]
    public void TestItems_Modelo_Maximo_60_Caracteres()
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
    public void TestItems_Categoria_Maxima_40_Caracteres()
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
    public void TestItems_Marca_Maxima_60_Caracteres()
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
    public void TestItem_Crear_AsignarIdAutoincremental_Correcto()
    {
        Item item1 = new Item { Titulo = "Item 1", Descripcion = "Desc 1" };
        Item item2 = new Item { Titulo = "Item 2", Descripcion = "Desc 2" };

        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreNotEqual(item1.Id, item2.Id);
    }
    
    [TestMethod]
    public void TestItem_Equals_ComparaPorId()
    {
        Item item1 = new Item { Titulo = "A", Descripcion = "B", Categoria = "Categoria", Marca = "Marca",  Modelo = "Modelo" };
        Item item2 = new Item { Titulo = "C", Descripcion = "D",  Categoria = "Categoria", Marca = "Marca", Modelo = "Modelo" };
        Item item3 = item1;

        Assert.IsTrue(item1.Equals(item3));
        Assert.IsFalse(item1.Equals(item2));
    }
 
    [TestMethod]
    public void TestItem_ResetIdCounter_ReiniciaIds()
    {
        var item1 = new Item { Titulo = "Uno", Descripcion = "Desc" , Categoria = "Categoria",  Marca = "Marca", Modelo = "Modelo" };
        var item2 = new Item { Titulo = "Dos", Descripcion = "Desc",  Categoria = "Categoria", Marca = "Marca", Modelo = "Modelo" };
        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);

        Item.ResetIdCounter();

        var item3 = new Item { Titulo = "Tres", Descripcion = "Desc"};
        Assert.AreEqual(1, item3.Id); // vuelve a empezar en 1
    }
   
    [TestMethod]
    public void TestItem_Equals_ConObjetoDeOtroTipo_DevuelveFalse()
    {
        Item item = new Item { Titulo = "A", Descripcion = "B", Marca = "Marca", Modelo = "Modelo", Categoria = "Categoria" };

        Assert.IsFalse(item.Equals("no soy un item"));

    }
    [TestMethod]
    public void TestItem_AccederGettersParaCobertura()
    {
        Item item = new Item
        {
            Titulo = "Titulo",
            Descripcion = "Descripcion",
            Marca = "Marca",
            Modelo = "Modelo",
            Categoria = "Categoria"
        };

        // Acceder explícitamente a todos los getters
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
    public void TestItems_Marca_Null_Ok()
    {
        Item item = new Item { Titulo = "T", Descripcion = "D", Marca = null };
        Assert.IsNull(item.Marca);
    }

    [TestMethod]
    public void TestItems_Modelo_Null_Ok()
    {
        Item item = new Item { Titulo = "T", Descripcion = "D", Modelo = null };
        Assert.IsNull(item.Modelo);
    }

    [TestMethod]
    public void TestItems_Categoria_Null_Ok()
    {
        Item item = new Item { Titulo = "T", Descripcion = "D", Categoria = null };
        Assert.IsNull(item.Categoria);
    }
    [TestMethod]
    public void TestItem_Editar_Titulo_Fallo()
    {
        // Arrange
        Item item = new Item("Titulo original", "Descripcion original");

        // Asignar un valor válido primero para cubrir el setter completo
        item.EditarTitulo("TituloValido");
        Assert.AreEqual("TituloValido", item.Titulo);

        // Act & Assert: asignar título nulo para provocar la excepción
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarTitulo(null);
        });

        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }

    [TestMethod]
    public void TestItem_Editar_Descripcion_Fallo()
    {
        // Arrange
        Item item = new Item("Titulo original", "Descripcion original");

        // Asignar un valor válido primero
        item.EditarDescripcion("DescripcionValida");
        Assert.AreEqual("DescripcionValida", item.Descripcion);

        // Act & Assert: asignar descripción vacía para provocar la excepción
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarDescripcion("");
        });

        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }

    [TestMethod]
    public void TestItem_Editar_Marca_Fallo()
    {
        // Arrange
        Item item = new Item("Titulo", "Descripcion");

        // Asignar un valor válido primero
        item.EditarMarca("MarcaValida");
        Assert.AreEqual("MarcaValida", item.Marca);

        // Act & Assert: asignar marca demasiado larga para provocar la excepción
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarMarca(new string('A', 61));
        });

        Assert.AreEqual("La marca no puede superar 60 caracteres.", exception.Message);
    }


    [TestMethod]
    public void TestItem_Editar_Modelo_Fallo()
    {
        // Arrange
        Item item = new Item("Titulo", "Descripcion");

        // Asignar un valor válido primero para cubrir el setter completo
        item.EditarModelo("ModeloValido");
        Assert.AreEqual("ModeloValido", item.Modelo);

        // Act & Assert: asignar modelo demasiado largo para provocar la excepción
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarModelo(new string('B', 61));
        });

        Assert.AreEqual("El modelo no puede superar 60 caracteres.", exception.Message);
    }

    [TestMethod]
    public void TestItem_Editar_Categoria_Fallo()
    {
        // Arrange
        Item item = new Item("Titulo", "Descripcion");

        // Asignar un valor válido primero
        item.EditarCategoria("CategoriaValida");
        Assert.AreEqual("CategoriaValida", item.Categoria);

        // Act & Assert: asignar categoria demasiado larga para provocar la excepción
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.EditarCategoria(new string('C', 41));
        });

        Assert.AreEqual("La categoria no puede superar 40 caracteres.", exception.Message);
    }
    
    [TestMethod]
    public void TestItem_Editar_Titulo_Correcto()
    {
        Item item = new Item("Titulo original", "Descripcion original");

        // Editar correctamente
        item.EditarTitulo("Nuevo Titulo");

        Assert.AreEqual("Nuevo Titulo", item.Titulo);
    }

    [TestMethod]
    public void TestItem_Editar_Descripcion_Correcto()
    {
        Item item = new Item("Titulo", "Descripcion original");

        item.EditarDescripcion("Nueva Descripcion");

        Assert.AreEqual("Nueva Descripcion", item.Descripcion);
    }

    [TestMethod]
    public void TestItem_Editar_Marca_Correcto()
    {
        Item item = new Item("Titulo", "Descripcion");

        item.EditarMarca("NuevaMarca");

        Assert.AreEqual("NuevaMarca", item.Marca);
    }

    [TestMethod]
    public void TestItem_Editar_Modelo_Correcto()
    {
        Item item = new Item("Titulo", "Descripcion");

        item.EditarModelo("NuevoModelo");

        Assert.AreEqual("NuevoModelo", item.Modelo);
    }

    [TestMethod]
    public void TestItem_Editar_Categoria_Correcto()
    {
        Item item = new Item("Titulo", "Descripcion");

        item.EditarCategoria("NuevaCategoria");

        Assert.AreEqual("NuevaCategoria", item.Categoria);
    }

    [TestMethod]
    public void TestItem_ModificarId_Correcto()
    {
        Item item = new Item("Titulo", "Descripcion");
        int idViejo = item.Id;
        int idNuevo = idViejo + 100;

        item.ModificarId(idNuevo);
        
        Assert.AreNotEqual(idViejo, item.Id);
        Assert.AreEqual(idNuevo, item.Id);
    }
    
    [TestMethod]
    public void TestItem_ModificarId_Id_Cero_Fallo()
    {
        Item item = new Item("Titulo", "Descripcion");
        int idNuevo = 0;

        
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            item.ModificarId(idNuevo);
        });

        Assert.AreEqual("El id no es valido", exception.Message);
    }
}