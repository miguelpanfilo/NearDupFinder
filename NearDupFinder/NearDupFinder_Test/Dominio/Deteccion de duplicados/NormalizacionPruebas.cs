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
    

    
    
}