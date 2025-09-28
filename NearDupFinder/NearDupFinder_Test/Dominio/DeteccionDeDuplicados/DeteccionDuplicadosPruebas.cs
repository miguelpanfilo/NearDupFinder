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
    
    [TestMethod]
    public void DetectarDuplicados_ScoreIgual060_AgregaComoPosibleDuplicado()
    {
        Item itemA = new Item
        {
            Titulo = "Notebook Lenovo L14",
            Descripcion = "alpha beta gamma delta epsilon zeta theta",
            Marca = "Lenovo",
            Modelo = "L14",
            Categoria = "Notebooks"
        };

        Item itemB = new Item
        {
            Titulo = "notebook lenovo l14",
            Descripcion = "alpha",
            Marca = "lenovo",
            Modelo = "",
            Categoria = "notebooks"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemB);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(0.60f, duplicados[0].Score, 1e-5f);
        Assert.AreEqual(TipoDuplicado.τ_alert, duplicados[0].Tipo);
    }
    
    [TestMethod]
    public void DetectarDuplicados_ScoreIgual075_AgregaComoDuplicadoSugerido()
    {
        Item itemA = new Item
        {
            Titulo = "Notebook Lenovo L14",
            Descripcion = "alpha beta gamma delta epsilon zeta theta",
            Marca = "",
            Modelo = "",
            Categoria = "Notebooks"
        };

        Item itemB = new Item
        {
            Titulo = "notebook lenovo l14",
            Descripcion = "alpha beta gamma delta epsilon zeta",
            Marca = "",
            Modelo = "",
            Categoria = "notebooks"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemB);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(0.75f, duplicados[0].Score, 1e-5f);
        Assert.AreEqual(TipoDuplicado.τ_dup, duplicados[0].Tipo);
    }
    
    [TestMethod]
    public void DetectarDuplicados_TokensCompartidos_InterseccionSinRepetidos()
    {
        Item itemA = new Item
        {
            Titulo = "alpha alpha beta",
            Descripcion = "rojo rojo verde azul",
            Marca = "Lenovo",
            Modelo = "L14",
            Categoria = "Notebooks"
        };

        Item itemB = new Item
        {
            Titulo = "beta alpha gamma alpha",
            Descripcion = "verde verde rojo",
            Marca = "lenovo",
            Modelo = "l14",
            Categoria = "notebooks"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemB);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        string[] esperadoTitulo = new[] { "alpha", "beta" };
        string[] esperadoDescripcion = new[] { "rojo", "verde" };

        CollectionAssert.AreEquivalent(esperadoTitulo, duplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(esperadoDescripcion, duplicados[0].TokensCompartidosDescripcion);
    }
    
    [TestMethod]
    public void DetectarDuplicados_MarcaYModelo_IgualdadBinaria_ExigeNoVacios()
    {
        Item itemA = new Item
        {
            Titulo = "Smartphone Samsung Galaxy S20",
            Descripcion = "uno dos",
            Marca = "Samsung",
            Modelo = "S20",
            Categoria = "Celulares"
        };

        Item itemB = new Item
        {
            Titulo = "smartphone samsung galaxy s20",
            Descripcion = "uno tres",
            Marca = "samsung",
            Modelo = "",
            Categoria = "celulares"
        };

        Catalogo catalogo = new Catalogo("Catalogo");
        catalogo.AgregarItem(itemA);
        catalogo.AgregarItem(itemB);

        List<Duplicados> duplicados = _sis.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(1, duplicados[0].ScoreMarca);
        Assert.AreEqual(0, duplicados[0].ScoreModelo);
    }
}