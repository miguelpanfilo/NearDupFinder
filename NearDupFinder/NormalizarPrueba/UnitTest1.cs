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


}