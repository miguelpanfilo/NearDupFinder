using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;

namespace NearDupFinder_Pruebas.Servicios.Duplicados;

[TestClass]
public class NormalizarPruebas
{
    private GestorDuplicados _gestorDuplicados = null!;

    private static Item CrearItem(string titulo, string desc, string marca, string modelo, string categoria)
    {
        Item item = new Item { Titulo = titulo, Descripcion = desc, Marca = marca, Modelo = modelo, Categoria = categoria };
        return item;
    }

    private static Catalogo CrearCatalogo(params Item[] items)
    {
        Catalogo catalogo = new Catalogo("Catalogo");
        foreach (Item item in items) catalogo.AgregarItem(item);
        return catalogo;
    }
    [TestInitialize]
    public void Setup()
    {
        var procesador = new ProcesadorTexto();
        _gestorDuplicados = new GestorDuplicados(procesador);
    }

    [TestMethod]
    public void NormalizarItem_TituloYDescripcionSoloSimbolos_LanzaExcepcion()
    {
        Item itemProblema = CrearItem("!@#$%", "***", "*", "##Modelo##", "!!Categoria!!");
        Catalogo catalogo = CrearCatalogo(itemProblema, CrearItem("ok", "ok", "m", "x", "cat"));

        System.InvalidOperationException error =
            Assert.ThrowsException<System.InvalidOperationException>(() => _gestorDuplicados.DetectarDuplicados(itemProblema, catalogo));

        Assert.AreEqual("El título y la descripción no pueden quedar vacío tras normalizar.", error.Message);
    }

    [TestMethod]
    public void Normalizar_TextoMayusculas_RetornarMinusculas()
    {
        Item itemA = CrearItem("LAPTOP", "POTENTE", "Marca", "Modelo", "Cat");
        Item itemB = CrearItem("laptop", "potente", "marca", "modelo", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "potente" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Normalizar_TextoConTildes_SeNormaliza()
    {
        Item itemA = CrearItem("ÁÉÍÓÚñÜ", "Descripción ÚNICA", "Ñandú", "MÓDeLo", "Tecnología");
        Item itemB = CrearItem("aeiounu", "descripcion unica", "nandu", "modelo", "tecnologia");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "aeiounu" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "descripcion", "unica" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Normalizar_TextoConSimbolos_SeEliminan()
    {
        Item itemA = CrearItem("lap{op", "compu%% buena", "to! shiba", "mo#de lo 1", "tecno");
        Item itemB = CrearItem("lap op", "compu buena", "to shiba", "mo de lo 1", "tecno");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "lap", "op" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "compu", "buena" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void Normalizar_TextoConEspaciosMultiples_ColapsaYRecorta()
    {
        Item itemA = CrearItem(" lapt op  ", "  compu  buena ", "  x  ", "  y  ", "cat");
        Item itemB = CrearItem("lapt op", "compu buena", "x", "y", "cat");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "lapt", "op" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "compu", "buena" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void NormalizarItem_ConCamposConMayusculas_SeNormalizaCorrectamente()
    {
        Item itemA = CrearItem("LAPTOP", "LAPTOP", "MarcaX", "Modelo1", "Categoria1");
        Item itemB = CrearItem("laptop", "laptop", "marcax", "modelo1", "categoria1");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void NormalizarItem_ConCamposConTildes_SeNormalizaCorrectamente()
    {
        Item itemA = CrearItem("Cómputañó", "Désc", "Ñandú", "Módelo", "Tecnología");
        Item itemB = CrearItem("computano", "desc", "nandu", "modelo", "tecnologia");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "computano" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "desc" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void NormalizarItem_EspaciosMultiples_ColapsaYRecorta()
    {
        Item itemA = CrearItem("  Lap_tóp   123!!  ", "de   sc", "  To!shIBa  ", " MÓDeLo   #1 ", "  TeCnología! ");
        Item itemB = CrearItem("lap top 123", "de sc", "to shiba", "modelo 1", "tecnologia");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "lap", "top", "123" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "de", "sc" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void NormalizarItem_ItemSoloConSimbolos_LanzaExcepcion()
    {
        Item item = CrearItem("!@#$%^&*()", "!!!!!", "***###", "###$$$", "!!@@");
        Catalogo catalogo = CrearCatalogo(item, CrearItem("ok", "ok", "m", "x", "cat"));

        System.InvalidOperationException error =
            Assert.ThrowsException<System.InvalidOperationException>(() => _gestorDuplicados.DetectarDuplicados(item, catalogo));

        Assert.AreEqual("El título y la descripción no pueden quedar vacío tras normalizar.", error.Message);
    }

    [TestMethod]
    public void NormalizarItem_MarcaModeloCategoriaSoloSimbolos_NoLanzaExcepcion()
    {
        Item itemA = CrearItem("Laptop", "Computadora potente", "!@#$$%", "###$$$", "!!@@");
        Item itemB = CrearItem("laptop", "computadora potente", "", "", "");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "computadora", "potente" }, resultado[0].TokensCompartidosDescripcion);
        Assert.AreEqual(0, resultado[0].ScoreMarca);
        Assert.AreEqual(0, resultado[0].ScoreModelo);
    }

    [TestMethod]
    public void NormalizarItem_TextoYaNormalizado_NoCambia()
    {
        Item itemA = CrearItem("laptop", "computadora potente", "marca", "modelo", "categoria");
        Item itemB = CrearItem("laptop", "computadora potente", "marca", "modelo", "categoria");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "computadora", "potente" }, resultado[0].TokensCompartidosDescripcion);
    }

    [TestMethod]
    public void NormalizarItem_SimbolosYEspacios_InicioFinalCorrecto()
    {
        Item itemA = CrearItem("!!!@@@   Laptop ***### ", " $$$ Computadora%% ", "***Marca***", " ##Modelo## ", " !!Categoria!! ");
        Item itemB = CrearItem("laptop", "computadora", "marca", "modelo", "categoria");

        Catalogo catalogo = CrearCatalogo(itemA, itemB);
        List<ParDuplicado> resultado = _gestorDuplicados.DetectarDuplicados(itemA, catalogo);

        CollectionAssert.AreEquivalent(new[] { "laptop" }, resultado[0].TokensCompartidosTitulo);
        CollectionAssert.AreEquivalent(new[] { "computadora" }, resultado[0].TokensCompartidosDescripcion);
    }
}