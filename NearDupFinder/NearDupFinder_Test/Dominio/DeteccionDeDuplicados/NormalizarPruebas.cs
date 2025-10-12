using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class NormalizarPruebas
{
    private readonly GestorDuplicados _gestorDuplicados = new GestorDuplicados();
    
    [TestMethod]
    public void Normalizar_TextoVacio_RetornaVacio()
    {
        string textoOriginal = "";

        var resultado = _gestorDuplicados.Normalizar(textoOriginal);

        Assert.AreEqual(string.Empty, resultado);
    }

    [TestMethod]
    public void Normalizar_TextoMayusculas_RetornarMinusculas()
    {
        string textoOriginal = "LAPTOP";

        var resultado = _gestorDuplicados.Normalizar(textoOriginal);

        Assert.AreEqual("laptop", resultado);
    }
    
    [TestMethod]
    public void Normalizar_TextoConTildes_SeNormaliza()
    {
        string textoOriginal = "ÁÉÍÓÚñÜ";

        var resultado = _gestorDuplicados.Normalizar(textoOriginal);

        Assert.AreEqual("aeiounu", resultado);
    }
    
    [TestMethod]
    public void Normalizar_TextoConSimbolos_SeEliminan()
    {
        string textoOriginal = "lap{op";

        var resultado = _gestorDuplicados.Normalizar(textoOriginal);

        Assert.AreEqual("lap op", resultado);
    }

    [TestMethod]
    public void Normalizar_TextoConEspaciosMultiples_ColapsaYRecorta()
    {
        string textoOriginal = " lapt op  ";

        var resultado = _gestorDuplicados.Normalizar(textoOriginal);

        Assert.AreEqual("lapt op", resultado);
    }

    [TestMethod]
    public void NormalizarItem_TituloYDescripcionSoloSimbolos_LanzaExcepcion()
    {
        var item = new Item
        {
            Titulo = "!@#$%",
            Descripcion = "***",
            Marca = "*",
            Modelo = "##Modelo##",
            Categoria = "!!Categoria!!"
        };

        var ex = Assert.ThrowsException<InvalidOperationException>(() => _gestorDuplicados.NormalizarItem(item));

        Assert.AreEqual(
            "El título y la descripción no pueden quedar vacío tras normalizar.",
            ex.Message
        );
    }
    
    [TestMethod]
    public void NormalizarItem_ConCamposConMayusculas_SeNormalizaCorrectamente()
    {
        var item = new Item
        {
            Titulo = "LAPTOP",
            Descripcion = "LAPTOP",
            Marca = "MarcaX",
            Modelo = "Modelo1",
            Categoria = "Categoria1"
        };

        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.TituloNormalizado);
        Assert.AreEqual("laptop", resultado.DescripcionNormalizada);
        Assert.AreEqual("marcax", resultado.MarcaNormalizada);
        Assert.AreEqual("modelo1", resultado.ModeloNormalizado);
    }
    
    [TestMethod]
    public void NormalizarItem_ConCamposConTildes_SeNormalizaCorrectamente()
    {
        var item = new Item
        {
            Titulo = "Cómputañó",
            Descripcion = "Désc",
            Marca = "Ñandú",
            Modelo = "Módelo",
            Categoria = "Tecnología"
        };

        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("computano", resultado.TituloNormalizado);
        Assert.AreEqual("desc", resultado.DescripcionNormalizada);
        Assert.AreEqual("nandu", resultado.MarcaNormalizada);
        Assert.AreEqual("modelo", resultado.ModeloNormalizado);
    }

    [TestMethod]
    public void NormalizarItem_EspaciosMultiples_ColapsaYRecorta()
    {
        var item = new Item
        {
            Titulo = "  Lap_tóp   123!!  ",
            Descripcion = "de   sc",
            Marca = "  To!shIBa  ",
            Modelo = " MÓDeLo   #1 ",
            Categoria = "  TeCnología! "
        };

        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("lap top 123", resultado.TituloNormalizado);
        Assert.AreEqual("de sc", resultado.DescripcionNormalizada);
            
        Assert.AreEqual("to shiba", resultado.MarcaNormalizada);
        Assert.AreEqual("modelo 1", resultado.ModeloNormalizado);
    }
    [TestMethod]
    public void NormalizarItem_ItemSoloConSimbolos_LanzaExcepcion()
    {
        var item = new Item
        {
            Titulo = "!@#$%^&*()",
            Descripcion = "!!!!!",
            Marca = "***###",
            Modelo = "###$$$",
            Categoria = "!!@@"
        };

        var ex = Assert.ThrowsException<InvalidOperationException>(() => _gestorDuplicados.NormalizarItem(item));

        Assert.AreEqual("El título y la descripción no pueden quedar vacío tras normalizar.", ex.Message);
    }

    [TestMethod]
    public void NormalizarItem_MarcaModeloCategoriaSoloSimbolos_NoLanzaExcepcion()
    {
        var item = new Item
        {
            Titulo = "Laptop",
            Descripcion = "Computadora potente",
            Marca = "!@#$$%",
            Modelo = "###$$$",
            Categoria = "!!@@"
        };
            
        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.TituloNormalizado);
        Assert.AreEqual("computadora potente", resultado.DescripcionNormalizada);
        Assert.AreEqual(string.Empty, resultado.MarcaNormalizada);
        Assert.AreEqual(string.Empty, resultado.ModeloNormalizado);
    }

    [TestMethod]
    public void NormalizarItem_TextoYaNormalizado_NoCambia()
    {
        var item = new Item
        {
            Titulo = "laptop",
            Descripcion = "computadora potente",
            Marca = "marca",
            Modelo = "modelo",
            Categoria = "categoria"
        };

        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.TituloNormalizado);
        Assert.AreEqual("computadora potente", resultado.DescripcionNormalizada);
        Assert.AreEqual("marca", resultado.MarcaNormalizada);
        Assert.AreEqual("modelo", resultado.ModeloNormalizado);
    }
    
    [TestMethod]
    public void NormalizarItem_SimbolosYEspacios_InicioFinalCorrecto()
    {
        var item = new Item
        {
            Titulo = "!!!@@@   Laptop ***### ",
            Descripcion = " $$$ Computadora%% ",
            Marca = "***Marca***",
            Modelo = " ##Modelo## ",
            Categoria = " !!Categoria!! "
        };

        var resultado = _gestorDuplicados.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.TituloNormalizado);
        Assert.AreEqual("computadora", resultado.DescripcionNormalizada);
        Assert.AreEqual("marca", resultado.MarcaNormalizada);
        Assert.AreEqual("modelo", resultado.ModeloNormalizado);
    }
}