using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases.Cluster;

[TestClass]
public class ClusterPruebas
{
    [TestMethod]
    public void ConfirmarDuplicado_CreaUnClusterConDosMiembros()
    {
        
        var cat = new Catalogo("X");
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };

        cat.AgregarItem(a);
        cat.AgregarItem(b);
        
        cat.ConfirmarDuplicado(a, b);

        Assert.AreEqual(1, cat.Clusters.Count());

        var cluster = cat.Clusters.First();
        
        var pertenecientesCluster = cluster.PertenecientesCluster.ToList();
        Assert.AreEqual(2, pertenecientesCluster.Count);
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            pertenecientesCluster);
    }
}