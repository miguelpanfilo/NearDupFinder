using NearDupFinder_Dominio.Clases;

namespace NormalizacionPruebas;

[TestClass]
public class NormalizacionPruebas
{
    [TestMethod]
    public void NormalizarItem_TituloConMayusculas_MinusculasCorrectas()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "LAPTOP",
            Marca = "MarcaX",
            Modelo = "Modelo1",
            Categoria = "Categoria1"
        };

        var resultado = sistema.NormalizarItem(item);

        Assert.AreEqual("laptop", resultado.Titulo);
    }
    
    [TestMethod]
    public void NormalizarItem_TituloConTildesYN_SeNormalizaCorrectamente()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "Cómputañó",
            Marca = "Ñandú",
            Modelo = "Módelo",
            Categoria = "Tecnología"
        };

        var resultado = sistema.NormalizarItem(item);

        Assert.AreEqual("computano", resultado.Titulo);
        Assert.AreEqual("nandu", resultado.Marca);
        Assert.AreEqual("modelo", resultado.Modelo);
        Assert.AreEqual("tecnologia", resultado.Categoria);
    }
    [TestMethod]
    public void NormalizarItem_TituloConSimbolosEspeciales_NormalizaCorrectamente()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "Lap_tóp!123#",
            Marca = "To!shIBa",
            Modelo = "MÓDeLo#1",
            Categoria = "TeCnología!"
        };

        var resultado = sistema.NormalizarItem(item);

        // Reemplazo de caracteres especiales  por espacios 
        Assert.AreEqual("lap top 123 ", resultado.Titulo);
        Assert.AreEqual("to shiba", resultado.Marca);
        Assert.AreEqual("modelo 1", resultado.Modelo);
        Assert.AreEqual("tecnologia ", resultado.Categoria);
        
        
        
    }
    
    [TestMethod]
    public void NormalizarItem_EspaciosMultiples_ColapsaYRecorta()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "  Lap_tóp   123!!  ",
            Marca = "  To!shIBa  ",
            Modelo = " MÓDeLo   #1 ",
            Categoria = "  TeCnología! "
        };

        var resultado = sistema.NormalizarItem(item);
        
        // Hasta ese momento, linea de texto sin tildes, minusculas, sin caracteres especiales

        Assert.AreEqual("lap top 123", resultado.Titulo);
        Assert.AreEqual("to shiba", resultado.Marca);
        Assert.AreEqual("modelo 1", resultado.Modelo); 
        Assert.AreEqual("tecnologia", resultado.Categoria);

    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void NormalizarItem_ItemSoloConSimbolos_LanzaExcepcion()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "!@#$%^&*()",
            Descripcion = "!!!!!",
            Marca = "***###",
            Modelo = "###$$$",
            Categoria = "!!@@"
        };
        
        //Tras normalizar, me tendria que dar cada propiedad vacia, lanzando una excepcion 

        var resultado = sistema.NormalizarItem(item);
    }



    
    
}