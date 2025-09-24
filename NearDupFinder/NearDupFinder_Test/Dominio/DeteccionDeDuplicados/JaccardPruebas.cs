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
}