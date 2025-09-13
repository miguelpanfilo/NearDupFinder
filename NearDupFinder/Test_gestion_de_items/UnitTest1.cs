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
    
}