using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class JaccardPruebas
{
    private readonly Sistema _sis = new Sistema(); 

    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_NoVacios()
    {
        string[] a = ["a", "b", "b" ]; 
        string[] b = [ "b", "c" ];
        
        int numTokens = _sis.CalcularNumTokensUnion(a, b);
        
        Assert.AreEqual(3, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_UnoVacio()
    {
        string[] a = ["a", "b", "b" ]; 
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(2, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_AmbosVacios_RetornaCero()
    {
        string[] a = [];
        string[] b = [];

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(0, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_UnoInvalido_RetornaMenosUno()
    {
        string[]? a = null;
        string[] b = ["x"];

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(-1, numTokens);
    }
    
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosInvalidos_RetornaMenosUno()
    {
        string[]? a = null;
        string[]? b = null;

        int numTokens = _sis.CalcularNumTokensUnion(a, b);

        Assert.AreEqual(-1, numTokens);
    }
}