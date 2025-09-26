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

    [TestMethod]
    public void CalcularScore_TituloYDescripcionUno_SinMarcaModelo_Retorna0_80()
    {
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 1f;
        float marcaEq = 0;
        float modeloEq = 0;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(0.80f, score, 1e-6f);    
    }

    [TestMethod]
    public void CalcularScore_TodoUno_Retorna1_00()
    {
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 1f;
        float marcaEq = 1f;
        float modeloEq = 1f;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(1.00f, score, 1e-6f);  
    }

    [TestMethod]
    public void CalcularScore_ValoresParciales_RetornaValorEsperado()
    {
        float jaccardTitulo = 0.5f;
        float jaccardDescripcion = 0.2f;
        float marcaEq = 1f;
        float modeloEq = 0f;
        
        float score = _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq,  modeloEq);
        
        Assert.AreEqual(0.395f, score, 1e-6f);  
    }

    [TestMethod]
    public void CalcularScore_JaccardTitulo_Negativo_LanzaArgumentOutOfRangeException()
    {
        float jaccardTitulo = -0.01f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }
    
    [TestMethod]
    public void CalcularScore_JaccardTitulo_MayorAUno_LanzaArgumentOutOfRangeException()
    {
        float jaccardTitulo = 2.0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sistema.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

}