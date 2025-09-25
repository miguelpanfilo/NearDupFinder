using NearDupFinder_Dominio.Clases;

namespace NormalizarPrueba;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Normalizar_TextoVacio_RetornaVacio()
    {
        var sistema = new Sistema();
        string textoOriginal = "";

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual(string.Empty, resultado);
    }
    [TestMethod]
    public void Normalizar_TextoNull_RetornaVacio()
    {
        var sistema = new Sistema();
        string textoOriginal = null;

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual(string.Empty, resultado);
    }

    [TestMethod]
    public void Normalizar_TextoMayusculas_RetornarMinusculas()
    {
        var sistema = new Sistema();
        string textoOriginal = "LAPTOP";

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual("laptop", resultado);
    }
    [TestMethod]
    public void Normalizar_TextoConTildes_SeNormaliza()
    {
        var sistema = new Sistema();
        string textoOriginal = "ÁÉÍÓÚñÜ";

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual("aeiounu", resultado);
    }
    [TestMethod]
    public void Normalizar_TextoConSimbolos_SeEliminan()
    {
        var sistema = new Sistema();
        string textoOriginal = "lap{op";

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual("lap op", resultado);
    }

    [TestMethod]
    public void Normalizar_TextoConEspaciosMultiples_ColapsaYRecorta()
    {
        var sistema = new Sistema();
        string textoOriginal = " lapt op  ";

        var resultado = sistema.Normalizar(textoOriginal);

        Assert.AreEqual("lapt op", resultado);
    }

    [TestMethod]
    public void NormalizarItem_TituloYDescripcionSoloSimbolos_LanzaExcepcion()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "!@#$%",
            Descripcion = "***",
            Marca = "*",
            Modelo = "##Modelo##",
            Categoria = "!!Categoria!!"
        };

        var ex = Assert.ThrowsException<InvalidOperationException>(() => sistema.NormalizarItem(item));

        Assert.AreEqual(
            "El título y la descripción no pueden quedar vacío tras normalizar.",
            ex.Message
        );
    }
    
    [TestMethod]

    public void NormalizarItem_TituloConMayusculas()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "LAPTOP",
            Descripcion = "LAPTOP",
            Marca = "MarcaX",
            Modelo = "Modelo1",
            Categoria = "Categoria1"
        };

        var resultado = sistema.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.Titulo);
        Assert.AreEqual("laptop", resultado.Descripcion);

        Assert.AreEqual("marcax", resultado.Marca);
        Assert.AreEqual("modelo1", resultado.Modelo);
        Assert.AreEqual("categoria1", resultado.Categoria);
    }


    [TestMethod]
    public void NormalizarItem_TituloConTildesYN_SeNormalizaCorrectamente()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "Cómputañó",
            Descripcion = "Désc",
            Marca = "Ñandú",
            Modelo = "Módelo",
            Categoria = "Tecnología"
        };

        var resultado = sistema.NormalizarItem(item);

        Assert.AreEqual("computano", resultado.Titulo);
        Assert.AreEqual("desc", resultado.Descripcion);
        Assert.AreEqual("nandu", resultado.Marca);
        Assert.AreEqual("modelo", resultado.Modelo);
        Assert.AreEqual("tecnologia", resultado.Categoria);
    }



}