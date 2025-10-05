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
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemB_Null_LanzaArgumentNullException()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d" };
        cat.AgregarItem(a);
        
        var ex = Assert.ThrowsException<ArgumentNullException>(() => cat.ConfirmarDuplicado(a, null));
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemNoPerteneceAlCatalogo_LanzaInvalidOperationException()
    {
        var cat = new Catalogo("Stock Tata");

        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };

        cat.AgregarItem(a);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => cat.ConfirmarDuplicado(a, b));
        StringAssert.Contains(ex.Message, "Uno o ambos ítems no pertenecen al catalogo");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_RepiteMismoPar_NoCreaNuevoCluster()
    {
        
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);
        
        cat.ConfirmarDuplicado(a, b);
        var cantidadClauster = cat.Clusters.Count();
        cat.ConfirmarDuplicado(a, b); 
        
        Assert.AreEqual(cantidadClauster, cat.Clusters.Count());
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_Transitivo_AgregaTerceroAlMismoCluster()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        var c = new Item { Titulo = "C", Descripcion = "d3" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);
        cat.AgregarItem(c);
        
        cat.ConfirmarDuplicado(a, b); 
        cat.ConfirmarDuplicado(b, c); 
        
        Assert.AreEqual(1, cat.Clusters.Count());
        var cluster = cat.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b, c },
            cluster.PertenecientesCluster.ToList()
        );
    }
}