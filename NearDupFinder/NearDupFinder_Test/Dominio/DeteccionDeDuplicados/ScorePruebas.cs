using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.DeteccionDeDuplicados;

[TestClass]
public class ScorePruebas
{
    [TestMethod]
    public void CalcularScore_TodosCeros_RetornaCero()
    {
        Sistema sis = new Sistema();
        float Jaccard_Titulo = 0;
        float Jaccard_Descripcion = 0;
        float Marca_Eq = 0;
        float Modelo_Eq = 0;

        float score = sis.CalcularScore(Jaccard_Titulo, Jaccard_Descripcion, Marca_Eq,  Modelo_Eq);
        
        Assert.AreEqual(score, 0.0f);
    }
}