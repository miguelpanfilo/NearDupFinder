using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Excepciones;

namespace Test_gestion_de_items;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestItems_Crear_Item_Ok()
    {
        Catalogo catalogo = new Catalogo
        {
            Titulo = "Titulo de catalogo"
        };
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Catalogo = catalogo
        };

        
        Assert.AreEqual("Soy un titulo", item.Titulo);
        Assert.AreEqual("Soy una descripcion", item.Descripcion);
        Assert.IsNotNull(item.Catalogo);
        Assert.AreEqual("Titulo de catalogo", item.Catalogo.Titulo);
            

    }
    [TestMethod]
   public void TestItems_Crear_Item_Sin_Catalogo()
    {
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Soy un titulo",
                Descripcion = "Soy una descripcion",
                Catalogo = null
           
            };
        });
        Assert.AreEqual("El Item debe tener un Catalogo.", exception.Message);
        
        
    }
    
    [TestMethod]
    
    public void TestItems_Crear_Item_Sin_Titulo()
    {
        Catalogo catalogo = new Catalogo();
       
        
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
            {
                Item item = new Item
                {
                    Titulo = "",  
                    Descripcion = "Soy una descripcion",
                    Catalogo = catalogo
                };
            });
        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Sin_Descripcion()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Soy un titulo",
                Descripcion = "",  
                Catalogo = catalogo
            };
        });

        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Titulo_Largo_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string titulo = new string('A', 121);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = titulo,
                Descripcion = "Descripcion",
                Catalogo = catalogo
            };
        });

        Assert.AreEqual("El Título no puede superar 120 caracteres.", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Descripcion_Largo_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string descripcion = new string('A', 401);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "titulo",
                Descripcion = descripcion,
                Catalogo = catalogo
            };
        });

        Assert.AreEqual("La descripcion no puede superar 400 caracteres.", exception.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Marca_Larga_Fallo()
    {
       Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
       string  marca = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "titulo",
                Descripcion = " Soy descripcion",
                Catalogo = catalogo,
                Marca = marca
            };
        });

        Assert.AreEqual("La marca no puede superar 60 caracteres.", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Modelo_Largo_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string  modelo = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "titulo",
                Descripcion = " Soy descripcion",
                Catalogo = catalogo,
                Modelo = modelo
            };
        });

        Assert.AreEqual("El modelo no puede superar 60 caracteres.", exception.Message);
    } 
    [TestMethod]
    public void TestItems_Crear_Item_Categoria_Larga_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string  categoria = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "titulo",
                Descripcion = " Soy descripcion",
                Catalogo = catalogo,
                Categoria = categoria
            };
        });

        Assert.AreEqual("La categoria no puede superar 40 caracteres.", exception.Message);
    } 
    [TestMethod]
    public void TestItems_Titulo_Obligatorio_Con_Maximo_120_Caracteres()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string tituloMax = new string('A', 120);

        Item item = new Item
        {
            Titulo = tituloMax,
            Descripcion = "Descripcion válida",
            Catalogo = catalogo
        };

        Assert.AreEqual(120, item.Titulo.Length);
        Assert.AreEqual(tituloMax, item.Titulo);
    }
    [TestMethod]
    public void TestItems_Modelo_Maximo_60_Caracteres()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string modeloMax = new string('D', 60);

        Item item = new Item
        {
            Titulo = "Titulo válido",
            Descripcion = "Descripcion válida",
            Catalogo = catalogo,
            Modelo = modeloMax
        };

        Assert.AreEqual(60, item.Modelo.Length);
        Assert.AreEqual(modeloMax, item.Modelo);
    }
    
    [TestMethod]
    public void TestItems_Categoria_Maxima_40_Caracteres()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string categoriaMax = new string('E', 40);

        Item item = new Item
        {
            Titulo = "Titulo válido",
            Descripcion = "Descripcion válida",
            Catalogo = catalogo,
            Categoria = categoriaMax
        };

        Assert.AreEqual(40, item.Categoria.Length);
        Assert.AreEqual(categoriaMax, item.Categoria);
    }
    [TestMethod]
    public void TestItems_Marca_Maxima_60_Caracteres()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };
        string marcaMax = new string('M', 60);

        Item item = new Item
        {
            Titulo = "Titulo válido",
            Descripcion = "Descripcion válida",
            Catalogo = catalogo,
            Marca = marcaMax
        };

        Assert.AreEqual(60, item.Marca.Length);
        Assert.AreEqual(marcaMax, item.Marca);
    }
    [TestMethod]
    public void TestItems_Titulo_Solo_Espacios_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "            ",  
                Descripcion = "Descripcion válida",
                Catalogo = catalogo
            };
        });

        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }
    
    [TestMethod]
    public void TestItems_Descripcion_Solo_Espacios_Fallo()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Titulo válido",
                Descripcion = "    ", 
                Catalogo = catalogo
            };
        });

        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }
    [TestMethod]
    public void TestCatalogo_Agregar_Item_Correcto()
    {
        Catalogo catalogo = new Catalogo { Titulo = "Catalogo Ejemplo" };

        Item item = new Item
        {
            Titulo = "Titulo obligatorio",
            Descripcion = "Descripcion obligatoria",
            Catalogo = catalogo
        };

    
        catalogo.AgregarItem(item);

        
        Assert.IsTrue(catalogo.items.Contains(item));
        Assert.AreEqual(1, catalogo.items.Count);
    }
    
    [TestMethod]
    public void CrearItem_AsignarIdAutoincremental()
    {
        Item item1 = new Item { Titulo = "Item 1", Descripcion = "Desc 1" };
        Item item2 = new Item { Titulo = "Item 2", Descripcion = "Desc 2" };

        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreNotEqual(item1.Id, item2.Id);
    }
    [TestMethod]
    public void ObtenerItemPorId_DevuelveItemCorrecto()
    {
        Catalogo catalogo = new Catalogo();

        Item item1 = new Item { Titulo = "Item 1", Descripcion = "Desc 1" };
        Item item2 = new Item { Titulo = "Item 2", Descripcion = "Desc 2" };

        catalogo.AgregarItem(item1);
        catalogo.AgregarItem(item2);

        Item buscado = catalogo.ObtenerItemPorId(2);

        Assert.AreEqual(item2, buscado);
        Assert.AreEqual("Item 2", buscado.Titulo);
    }
    [TestMethod]
    public void EliminarItem_PorId_ItemEliminadoCorrectamente()
    {
        
        Catalogo catalogo = new Catalogo("Catalogo Ejemplo");
        Item item1 = new Item { Titulo = "Item 1", Descripcion = "Desc 1" };
        Item item2 = new Item { Titulo = "Item 2", Descripcion = "Desc 2" };

        catalogo.AgregarItem(item1);
        catalogo.AgregarItem(item2);

        
        catalogo.EliminarItem(item1.Id);

        
        Assert.IsFalse(catalogo.items.Contains(item1)); 
        Assert.AreEqual(1, catalogo.items.Count);       
        Assert.IsTrue(catalogo.items.Contains(item2)); 
    }

}