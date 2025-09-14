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
    [ExpectedException(typeof(ArgumentException))]
    public void TestItems_Crear_Item_Sin_Catalogo()
    {
       
        Item item = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Catalogo = null
           
        };
        
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
    
    
}