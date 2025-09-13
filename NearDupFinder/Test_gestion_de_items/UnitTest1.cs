using NearDupFinder_Dominio;

namespace Test_gestion_de_items;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestItems_CrearItemOk()
    {
        Catalogo catalogo = new Catalogo
        {
            Titulo = "Titulo de catalogo"
        };
        Item i = new Item
        {
            Titulo = "Soy un titulo",
            Descripcion = "Soy una descripcion",
            Catalogo = catalogo
        };

        
        Assert.AreEqual("Soy un titulo", i.Titulo);
        Assert.AreEqual("Soy una descripcion", i.Descripcion);
        Assert.IsNotNull(i.Catalogo);
        Assert.AreEqual("Titulo de catalogo", i.Catalogo.Titulo);
            

    }
}