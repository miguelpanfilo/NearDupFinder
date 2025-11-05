using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[TestClass]
public class DeteccionDuplicadosPruebas
{
    private GestorDuplicados _gestorDuplicados = null!;
    private GestorCatalogos gestorCatalogos = null!;
    
    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria) =>
        new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };

    private static Catalogo CrearCatalogo(params Item[] items)
    {
        var catalogo = new Catalogo("Catalogo");
        foreach (var item in items) catalogo.AgregarItem(item);
        return catalogo;
    }
    
    [TestInitialize]
    public void Setup()
    {
        var procesador = new ProcesadorTexto();
        _gestorDuplicados = new GestorDuplicados(procesador);
    }

    [TestMethod]
    public void DetectarDuplicados_CatalogoVacio_DevuelveListaVacia()
    {
        Item itemAComparar = CrearItem("a", "b", "", "", "x");
        Catalogo catalogo = CrearCatalogo();

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados.Count);
    }

    [TestMethod]
    public void DetectarDuplicados_OmiteMismoItem_NoAgregaEsePar()
    {
        Item itemAComparar = CrearItem("Notebook Lenovo L14", "Intel i5 8GB 256GB SSD", "Lenovo", "L14", "Notebooks");
        Item itemPosibleDuplicado = CrearItem("notebook lenovo l14", "intel i5 8gb 256gb ssd", "lenovo", "l14", "notebooks");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemPosibleDuplicado);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(1, duplicados.Count);
        Assert.AreEqual(itemPosibleDuplicado.Id, duplicados[0].ItemPosibleDuplicado.Id);
    }
    
    [TestMethod]
    public void DetectarDuplicados_IgualdadBinaria_RetornaUnoOCero()
    {
        Item itemAComparar = CrearItem("Smartphone Samsung Galaxy S20", "uno dos", "Samsung", "S20", "Celulares");
        Item itemPosibleDuplicado = CrearItem("smartphone samsung galaxy s20", "uno tres", "samsung", "", "celulares");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemPosibleDuplicado);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(1, duplicados[0].ScoreMarca);
        Assert.AreEqual(0, duplicados[0].ScoreModelo);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreMenorAUmbralAlerta_NoAgrega()
    {
        Item itemAComparar = CrearItem("a", "b", "c", "d", "e");
        Item item2AComparar = CrearItem("f", "g", "h", "i", "j");
        Catalogo catalogo = CrearCatalogo(itemAComparar, item2AComparar);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados.Count);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreDentroDelUmbralAlerta_AgregaComoAlertaDuplicado()
    {
        Item itemAComparar = CrearItem("Notebook Lenovo L14", "alpha beta gamma delta epsilon zeta theta", "Lenovo", "L14", "Notebooks");
        Item itemPosibleDuplicado = CrearItem("notebook lenovo l14", "alpha", "lenovo", "", "notebooks");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemPosibleDuplicado);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(TipoDuplicado.AlertaDuplicado, duplicados[0].Tipo);
    }

    [TestMethod]
    public void DetectarDuplicados_ScoreDentroDelUmbralPosible_AgregaComoPosibleDuplicado()
    {
        Item itemAComparar = CrearItem("Notebook Lenovo L14", "alpha beta gamma delta epsilon zeta", "", "", "Notebooks");
        Item itemB = CrearItem("notebook lenovo l14", "alpha beta gamma delta epsilon zeta", "", "", "notebooks");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemB);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(TipoDuplicado.PosibleDuplicado, duplicados[0].Tipo);
    }

    [TestMethod]
    public void DetectarDuplicados_TokensCompartidos_InterseccionSinRepetidos()
    {
        Item itemAComparar = CrearItem("alpha alpha beta", "rojo rojo verde azul", "Lenovo", "L14", "Notebooks");
        Item itemB = CrearItem("beta alpha gamma alpha", "verde verde rojo", "lenovo", "l14", "notebooks");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemB);
        var esperadoTitulo = new[] { "alpha", "beta" };
        var esperadoDescripcion = new[] { "rojo", "verde" };

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);
        
        CollectionAssert.AreEquivalent(esperadoTitulo, duplicados[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(esperadoDescripcion, duplicados[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void DetectarDuplicados_TokensCompartidos_SiNoCoinciden_QuedanVacios()
    {
        Item itemAComparar = CrearItem("alpha beta", "rojo verde", "Lenovo", "L14", "n");
        Item itemB = CrearItem("alpha beta", "azul negro", "lenovo", "l14", "n");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemB);

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);

        Assert.AreEqual(0, duplicados[0].TokensCompartidosDescripcion.Length);
    }

    [TestMethod]
    public void DetectarDuplicados_Ordenamiento_ScoreDesc_IdAsc()
    {
        Item itemAComparar  = CrearItem("Notebook Lenovo L14", "uno dos", "Lenovo", "L14", "Notebooks");
        Item itemPosibleDuplicado1 = CrearItem("notebook lenovo l14", "uno dos", "lenovo", "l14", "notebooks");
        Item itemPosibleDuplicado2 = CrearItem("Notebook Lenovo L14", "uno dos", "Lenovo", "L14", "Notebooks");
        Item itemC  = CrearItem("notebook lenovo l14", "uno", "", "", "notebooks");
        Catalogo catalogo = CrearCatalogo(itemAComparar, itemC, itemPosibleDuplicado1, itemPosibleDuplicado2);
        var esperado = new List<int> { itemPosibleDuplicado1.Id, itemPosibleDuplicado2.Id, itemC.Id };

        List<ParDuplicado> duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);
        var actual = duplicados.Select(d => d.ItemPosibleDuplicado.Id).ToList();
        
        CollectionAssert.AreEqual(esperado, actual);
    }
    
    [TestMethod]
    public void DetectarDuplicados_ItemConNombreNoTokenizable_LanzaExcepcion()
    {
        Item itemAComparar = CrearItem("@", "@", "", "", "x");
        Catalogo catalogo = CrearCatalogo(itemAComparar);
        
        var error = Assert.ThrowsException<InvalidOperationException>(() => _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo));
        Assert.AreEqual("El título y la descripción no pueden quedar vacío tras normalizar.", error.Message);
    }
    [TestMethod]
    public void DetectarDuplicados_UsaProcesamientoDeTexto_StopwordsYStemming()
    {
        var itemA = CrearItem(
            "Canción rápida",
            "Los jugadores estaban jugando en el sistema nuevo",
            "Sony", "X1", "Audio"
        );

        var itemB = CrearItem(
            "Cancion rapida",
            "jugador juega sistemas nuevos",
            "Sony", "X1", "Audio"
        );

        Catalogo catalogo = CrearCatalogo(itemA, itemB);

        var procesador = new ProcesadorTexto();
        var gestor = new GestorDuplicados(procesador);

        List<ParDuplicado> duplicados = gestor.DetectarDuplicados(itemA, catalogo);

        Assert.AreEqual(1, duplicados.Count,
            "Debería detectar al menos un posible duplicado, ya que ambos ítems son equivalentes lingüísticamente.");

        string[] tokensTitulo = duplicados[0].TokensCompartidosTitulo;

        CollectionAssert.Contains(tokensTitulo, "cancion",
            "El token 'canción' debería haberse reducido correctamente a 'cancion' tras quitar tildes y aplicar stemming.");

        CollectionAssert.Contains(tokensTitulo, "rap",
            "El token 'rápida' debería haberse reducido correctamente a 'rap' tras aplicar stemming.");
    }

}