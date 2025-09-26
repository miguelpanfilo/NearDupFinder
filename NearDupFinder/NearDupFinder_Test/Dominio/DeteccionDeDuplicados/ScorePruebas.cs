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

    [TestMethod]
    public void CalcularScore_SoloTituloUno_Retorna0_45()
    {
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 0;
        float marcaEq = 0;
        float modeloEq = 0;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(score, 0.45f);
    }

    [TestMethod]
    public void CalcularScore_SoloDescripcionUno_Retorna0_35()
    {
        float jaccardTitulo = 0;
        float jaccardDescripcion = 1f;
        float marcaEq = 0;
        float modeloEq = 0;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(score, 0.35f);
    }

    [TestMethod]
    public void CalcularScore_SoloMarcaUno_Retorna0_10()
    {
        float jaccardTitulo = 0;
        float jaccardDescripcion = 0;
        float marcaEq = 1f;
        float modeloEq = 0;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(score, 0.10f);
    }

    [TestMethod]
    public void CalcularScore_SoloModeloUno_Retorna0_10()
    {
        float jaccardTitulo = 0;
        float jaccardDescripcion = 0;
        float marcaEq = 0;
        float modeloEq = 1f;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(score, 0.10f);
    }
}