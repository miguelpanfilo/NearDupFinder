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
    public void NormalizarItem_ItemSoloConSimbolos_LanzaExcepcionConMensaje()
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

        var ex = Assert.ThrowsException<InvalidOperationException>(() => sistema.NormalizarItem(item));

        Assert.AreEqual("El título o la descripción no puede quedar vacío tras normalizar.", ex.Message);
    }

    
    [TestMethod]
    public void NormalizarItem_MarcaModeloCategoriaVacios_NoLanzaExcepcion()
    {
        var sistema = new Sistema();
        var item = new Item
        {
            Titulo = "Laptop",
            Descripcion = "Computadora potente",
            Marca = "!@#$$%",   // se normaliza a vacío
            Modelo = "###$$$",  // se normaliza a vacío
            Categoria = "!!@@"  // se normaliza a vacío
        };

        // No debe lanzar excepción
        var resultado = sistema.NormalizarItem(item);

        // Verificar normalización
        Assert.AreEqual("laptop", resultado.Titulo);
        Assert.AreEqual("computadora potente", resultado.Descripcion);
        Assert.AreEqual(string.Empty, resultado.Marca);
        Assert.AreEqual(string.Empty, resultado.Modelo);
        Assert.AreEqual(string.Empty, resultado.Categoria);
    }


   


    
    
}