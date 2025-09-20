using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Excepciones;

namespace Test_gestion_de_items;

[TestClass]
public class UnitTest1
{  
    [TestInitialize]
    public void Setup()
    {
        Item.ResetIdCounter();
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
    
    public void TestItems_Crear_Item_Sin_Titulo()
    {
    
       
        
        ItemException exception = Assert.ThrowsException<ItemException>(() =>
            {
                Item item = new Item
                {
                    Titulo = "",  
                    Descripcion = "Soy una descripcion",
                    Marca = "Marca",
                    Modelo = "Modelo",
                    Categoria = "Categoria"
                };
            });
        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Sin_Descripcion()
    {
        

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Soy un titulo",
                Descripcion = "",  
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Titulo_Largo_Fallo()
    {
        string titulo = new string('A', 121);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = titulo,
                Descripcion = "Descripcion",
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("El Título no puede superar 120 caracteres.", exception.Message);
    }
    [TestMethod]
    public void TestItems_Crear_Item_Descripcion_Largo_Fallo()
    {
        string descripcion = new string('A', 401);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Titulo",
                Descripcion = descripcion,
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("La descripcion no puede superar 400 caracteres.", exception.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Marca_Larga_Fallo()
    {
       string  marca = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Titulo",
                Descripcion = "Descripcion",
                Marca = marca,
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("La marca no puede superar 60 caracteres.", exception.Message);
    }
    
    [TestMethod]
    public void TestItems_Crear_Item_Modelo_Largo_Fallo()
    {
        string  modelo = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>( () =>
        {
            Item item = new Item
            { 
                Titulo = "Titulo",
                Descripcion = "descripcion",
                Marca = "Marca",
                Modelo = modelo,
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("El modelo no puede superar 60 caracteres.", exception.Message);
    } 
    
    [TestMethod]
    public void TestItems_Crear_Item_Categoria_Larga_Fallo()
    {
        string  categoria = new string('A', 61);

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Titulo",
                Descripcion = "descripcion",
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = categoria,
                
            };
        });

        Assert.AreEqual("La categoria no puede superar 40 caracteres.", exception.Message);
    } 
    [TestMethod]
    public void TestItems_Titulo_Obligatorio_Con_Maximo_120_Caracteres()
    {
        string tituloMax = new string('A', 120);

        Item item = new Item
        {
            Titulo = tituloMax,
            Descripcion = "Descripcion válida",
            Marca = "Marca",
            Modelo = "Modelo",  
            Categoria = "Categoria"
            
        };

        Assert.AreEqual(120, item.Titulo.Length);
        Assert.AreEqual(tituloMax, item.Titulo);
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
    public void TestItems_Titulo_Null_Fallo()
    {

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = null,
                Descripcion = "Descripcion válida",
                Marca = "marca",
                Modelo = "Modelo",
                Categoria = "categoria"
            };
        });

        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }

    [TestMethod]
    public void TestItems_Titulo_Vacio_Fallo()
    {

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "",
                Descripcion = "Descripcion válida",
                Marca = "marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }

    [TestMethod]
    public void TestItems_Titulo_Solo_Espacios_Fallo()
    {

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "    ",  // solo espacios
                Descripcion = "Descripcion válida",
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("El Título es obligatorio", exception.Message);
    }

    
    [TestMethod]
    public void TestItems_Descripcion_Solo_Espacios_Fallo()
    {

        ItemException exception = Assert.ThrowsException<ItemException>(() =>
        {
            Item item = new Item
            {
                Titulo = "Titulo válido",
                Descripcion = "    ", 
                Marca = "Marca",
                Modelo = "Modelo",
                Categoria = "Categoria"
            };
        });

        Assert.AreEqual("La Descripción es obligatoria.", exception.Message);
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
    
    
   

}