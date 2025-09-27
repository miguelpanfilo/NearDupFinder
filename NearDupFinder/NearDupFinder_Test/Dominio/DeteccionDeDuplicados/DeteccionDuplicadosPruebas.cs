using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class DeteccionDuplicadosPruebas
{
    private readonly Sistema _sis = new Sistema();

    [TestMethod]
    public void DetectarDuplicados_OmiteMismoItem_NoAgregaEsePar()
    {
        Item itemA = new Item
        {
            Titulo = "Notebook Lenovo L14",
            Descripcion = "Intel i5 8GB 256GB SSD",
            Marca = "Lenovo",
            Modelo = "L14",
            Categoria = "Notebooks"
        };

        Item itemB = new Item
        {
            Titulo = "notebook lenovo l14",
            Descripcion = "intel i5 8gb 256gb ssd",
            Marca = "lenovo",
            Modelo = "l14",
            Categoria = "notebooks"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemB);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(1, duplicados.Count);
        Assert.AreEqual(itemB.Id, duplicados[0].ItemB.Id);
    }
    
    [TestMethod]
    public void DetectarDuplicados_ScoreMenor060_NoAgrega()
    {
        Item itemA = new Item
        {
            Titulo = "Notebook Lenovo L14",
            Descripcion = "Intel i5 8GB 256GB SSD",
            Marca = "Lenovo",
            Modelo = "L14",
            Categoria = "Notebooks"
        };

        Item itemC = new Item
        {
            Titulo = "Cafetera Espresso 15 bar",
            Descripcion = "bomba italiana metalica",
            Marca = "Philips",
            Modelo = "EP1220",
            Categoria = "Cocina"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemC);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(0, duplicados.Count);
    }
}