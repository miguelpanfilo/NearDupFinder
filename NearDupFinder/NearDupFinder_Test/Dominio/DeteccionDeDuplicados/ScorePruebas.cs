using NearDupFInder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class ScorePruebas
{
    [TestMethod]
    public void CalcularScore_TodosCeros_RetornaCero()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.0f, score);
    }

    [TestMethod]
    public void CalcularScore_SoloTituloUno_Retorna0_45()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.45f, score);
    }

    [TestMethod]
    public void CalcularScore_SoloDescripcionUno_Retorna0_35()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 1f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.35f, score);
    }

    [TestMethod]
    public void CalcularScore_SoloMarcaUno_Retorna0_10()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 1f;
        float modeloEq = 0f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.10f, score);
    }

    [TestMethod]
    public void CalcularScore_SoloModeloUno_Retorna0_10()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 1f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.10f, score);
    }

    [TestMethod]
    public void CalcularScore_TituloYDescripcionUno_SinMarcaModelo_Retorna0_80()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 1f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(0.80f, score, 1e-6f);
    }

    [TestMethod]
    public void CalcularScore_TodoUno_Retorna1_00()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 1f;
        float jaccardDescripcion = 1f;
        float marcaEq = 1f;
        float modeloEq = 1f;

        float score = gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);

        Assert.AreEqual(1.00f, score, 1e-6f);
    }

    [TestMethod]
    public void CalcularScore_JaccardTitulo_Negativo_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = -0.01f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

    [TestMethod]
    public void CalcularScore_JaccardTitulo_MayorAUno_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 2.0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

    [TestMethod]
    public void CalcularScore_JaccardDescripcion_Negativo_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = -1.0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

    [TestMethod]
    public void CalcularScore_JaccardDescripcion_MayorAUno_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 2.0f;
        float marcaEq = 0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

    [TestMethod]
    public void CalcularScore_MarcaEq_NoBinaria_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 2.0f;
        float modeloEq = 0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }

    [TestMethod]
    public void CalcularScore_ModeloEq_NoBinaria_LanzaArgumentOutOfRangeException()
    {
        var gestor = new GestorDuplicados();
        float jaccardTitulo = 0f;
        float jaccardDescripcion = 0f;
        float marcaEq = 0f;
        float modeloEq = 2.0f;

        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            gestor.CalcularScore(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq));
    }
}
