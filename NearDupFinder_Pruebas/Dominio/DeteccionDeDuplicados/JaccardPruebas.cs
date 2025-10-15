using NearDupFInder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Dominio.DeteccionDeDuplicados;

[TestClass]
public class JaccardPruebas
{
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosVacios_DevuelveCero()
    {
        var gestor = new GestorDuplicados();
        string[] a = [];
        string[] b = [];

        int numTokens = gestor.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(0, numTokens);
    }

    [TestMethod]
    public void CalcularNumTokensUnion_UnoVacio_DevuelveCardinalDelOtro()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["a", "b", "b"];
        string[] b = [];

        int numTokens = gestor.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(2, numTokens);
    }

    [TestMethod]
    public void CalcularNumTokensUnion_AmbosNoVacios_CuentaUnicos()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["a", "b", "b"];
        string[] b = ["b", "c"];

        int numTokens = gestor.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(3, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosVacios_DevuelveCero()
    {
        var gestor = new GestorDuplicados();
        string[] a = [];
        string[] b = [];

        int numTokens = gestor.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(0, numTokens);
    }

    [TestMethod]
    public void CalcularNumTokensInterseccion_UnoVacio_DevuelveCero()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["a", "b"];
        string[] b = [];

        int numTokens = gestor.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(0, numTokens);
    }

    [TestMethod]
    public void CalcularNumTokensInterseccion_AmbosNoVacios_CuentaUnicosComunes()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["a", "b", "b"];
        string[] b = ["b", "c", "b"];

        int numTokens = gestor.CalcularNumTokensInterseccion(a, b);

        Assert.AreEqual(1, numTokens);
    }
    
    [TestMethod]
    public void CalcularJaccard_AmbosVacios_DevuelveCero()
    {
        var gestor = new GestorDuplicados();
        string[] a = [];
        string[] b = [];

        float valorJaccard = gestor.CalcularJaccard(a, b);

        Assert.AreEqual(0f, valorJaccard);
    }

    [TestMethod]
    public void CalcularJaccard_UnoVacio_DevuelveCero()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["x", "y"];
        string[] b = [];

        float valorJaccard = gestor.CalcularJaccard(a, b);

        Assert.AreEqual(0f, valorJaccard);
    }

    [TestMethod]
    public void CalcularJaccard_AmbosNoVacios_RetornaValorEsperado()
    {
        var gestor = new GestorDuplicados();
        string[] a = ["a"];
        string[] b = ["a", "b"];

        float valorJaccard = gestor.CalcularJaccard(a, b);

        Assert.AreEqual(0.5f, valorJaccard);
    }
}
