using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class ScorePruebas
{
    private Sistema _sistema = new Sistema();
    [TestMethod]
    public void CalcularScore_TodosCeros_RetornaCero()
    {
        float jaccardTitulo = 0;
        float jaccardDescripcion = 0;
        float marcaEq = 0;
        float modeloEq = 0;

        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(score, 0.0f);
    }
}