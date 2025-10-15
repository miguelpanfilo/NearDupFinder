using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio;
using NearDupFInder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios;

[TestClass]
public class DeteccionDuplicadosPruebas
{
    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria) =>
        new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };

    private static Catalogo CrearCatalogoNuevo(params Item[] items)
    {
        var catalogo = new Catalogo("Catalogo");
        foreach (var item in items) catalogo.AgregarItem(item);
        return catalogo;
    }

    [TestMethod]
    public void DetectarDuplicados_CatalogoVacio_DevuelveListaVacia()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("a", "b", "", "", "x");
        var catalogo = CrearCatalogoNuevo();

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados.Count);
    }

    [TestMethod]
    public void DetectarDuplicados_OmiteMismoItem_NoAgregaEsePar()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("Notebook Lenovo L14", "Intel i5 8GB 256GB SSD", "Lenovo", "L14", "Notebooks");
        var itemPosibleDuplicado = CrearItem("notebook lenovo l14", "intel i5 8gb 256gb ssd", "lenovo", "l14", "notebooks");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemPosibleDuplicado);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(1, duplicados.Count);
        Assert.AreEqual(itemPosibleDuplicado.Id, duplicados[0].ItemPosibleDuplicado.Id);
    }
    
    [TestMethod]
    public void DetectarDuplicados_IgualdadBinaria_RetornaUnoOCero()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("Smartphone Samsung Galaxy S20", "uno dos", "Samsung", "S20", "Celulares");
        var itemPosibleDuplicado = CrearItem("smartphone samsung galaxy s20", "uno tres", "samsung", "", "celulares");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemPosibleDuplicado);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(1, duplicados[0].ScoreMarca);
        Assert.AreEqual(0, duplicados[0].ScoreModelo);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreMenorAUmbralAlerta_NoAgrega()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("Notebook Lenovo L14", "Intel i5 8GB 256GB SSD", "Lenovo", "L14", "Notebooks");
        var itemC = CrearItem("Cafetera Espresso 15 bar", "bomba italiana metalica", "Philips", "EP1220", "Cocina");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemC);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados.Count);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreDentroDelUmbralAlerta_AgregaComoAlertaDuplicado()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("Notebook Lenovo L14", "alpha beta gamma delta epsilon zeta theta", "Lenovo", "L14", "Notebooks");
        var itemPosibleDuplicado = CrearItem("notebook lenovo l14", "alpha", "lenovo", "", "notebooks");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemPosibleDuplicado);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(TipoDuplicado.AlertaDuplicado, duplicados[0].Tipo);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreDentroDelUmbralPosible_AgregaComoPosibleDuplicado()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("Notebook Lenovo L14", "alpha beta gamma delta epsilon zeta", "", "", "Notebooks");
        var itemB = CrearItem("notebook lenovo l14", "alpha beta gamma delta epsilon zeta", "", "", "notebooks");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemB);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(TipoDuplicado.PosibleDuplicado, duplicados[0].Tipo);
    }

    [TestMethod]
    public void DetectarDuplicados_TokensCompartidos_InterseccionSinRepetidos()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("alpha alpha beta", "rojo rojo verde azul", "Lenovo", "L14", "Notebooks");
        var itemB = CrearItem("beta alpha gamma alpha", "verde verde rojo", "lenovo", "l14", "notebooks");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemB);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        var esperadoTitulo = new[] { "alpha", "beta" };
        var esperadoDescripcion = new[] { "rojo", "verde" };
        CollectionAssert.AreEquivalent(esperadoTitulo, duplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(esperadoDescripcion, duplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void DetectarDuplicados_TokensCompartidos_SiNoCoinciden_QuedanVacios()
    {
        var sis = new Sistema();
        var itemAComparar = CrearItem("alpha beta", "rojo verde", "Lenovo", "L14", "n");
        var itemB = CrearItem("alpha beta", "azul negro", "lenovo", "l14", "n");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemB);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados[0].TokensCompartidosDescripcion.Length);
    }

    [TestMethod]
    public void DetectarDuplicados_Ordenamiento_ScoreDesc_IdAsc()
    {
        var sis = new Sistema();
        var itemAComparar  = CrearItem("Notebook Lenovo L14", "uno dos", "Lenovo", "L14", "Notebooks");
        var itemPosibleDuplicado1 = CrearItem("notebook lenovo l14", "uno dos", "lenovo", "l14", "notebooks");
        var itemPosibleDuplicado2 = CrearItem("Notebook Lenovo L14", "uno dos", "Lenovo", "L14", "Notebooks");
        var itemC  = CrearItem("notebook lenovo l14", "uno", "", "", "notebooks");
        var catalogo = CrearCatalogoNuevo(itemAComparar, itemC, itemPosibleDuplicado1, itemPosibleDuplicado2);

        var duplicados = sis.DetectarDuplicados(itemAComparar, catalogo);

        var esperado = new List<int> { itemPosibleDuplicado1.Id, itemPosibleDuplicado2.Id, itemC.Id };
        var actual = duplicados.Select(d => d.ItemPosibleDuplicado.Id).ToList();
        CollectionAssert.AreEqual(esperado, actual);
    }
}
