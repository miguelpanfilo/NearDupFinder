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


    
    
}