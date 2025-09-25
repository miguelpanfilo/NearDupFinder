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

}