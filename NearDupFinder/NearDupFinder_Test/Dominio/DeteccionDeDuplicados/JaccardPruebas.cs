using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class JaccardPruebas
{
    [TestMethod]
    public void CalcularNumTokensUnion_AmbosValidos_NoVacios()
    {
        Sistema sis = new Sistema(); 
        var a = new[] { "a", "b", "b" }; 
        var b = new[] { "b", "c" };
        
        var actual = sis.CalcularNumTokensUnion(a, b);
        
        Assert.AreEqual(3, actual);
    }
}