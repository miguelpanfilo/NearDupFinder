using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

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
    
    [TestMethod]
    public void ConfirmarDuplicado_ConectaDosClusters_DebeUnirlos()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        var c = new Item { Titulo = "C", Descripcion = "d3" };
        var d = new Item { Titulo = "D", Descripcion = "d4" };

        cat.AgregarItem(a);
        cat.AgregarItem(b);
        cat.AgregarItem(c);
        cat.AgregarItem(d);
        
        cat.ConfirmarDuplicado(a, b);
        
        cat.ConfirmarDuplicado(c, d);
        
        Assert.AreEqual(2, cat.Clusters.Count());
        
        cat.ConfirmarDuplicado(b, d);
        
        Assert.AreEqual(1, cat.Clusters.Count());
        var clusterUnico = cat.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b, c, d },
            clusterUnico.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoParEnOrdenInverso_NoCreaNuevoCluster()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);
        
        cat.ConfirmarDuplicado(a, b);
        cat.ConfirmarDuplicado(b, a);

        
        Assert.AreEqual(1, cat.Clusters.Count());
        var cluster = cat.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoItem_NoCreaCluster()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d" };
        cat.AgregarItem(a);

        cat.ConfirmarDuplicado(a, a);

        Assert.AreEqual(0, cat.Clusters.Count(), "No debe crearse un cluster cuando el par es (a,a).");
    }
    
    [TestMethod]
    public void ActualizarCanonico_ClusterQuedaVacio_NoLanzaExcepcion()
    {
        var a = new Item { Titulo = "A", Descripcion = "desc corta" };
        var cluster = new Cluster(1, new HashSet<Item> { a });
        
        cluster.Remover(a);

        Assert.IsNotNull(cluster);
        Assert.IsTrue(!cluster.PertenecientesCluster.Any());
    }
    
    [TestMethod]
    public void QuitarDeCluster_DejaSinClusterSiQuedaUnSoloItem()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);

        cat.ConfirmarDuplicado(a, b);
        Assert.AreEqual(1, cat.Clusters.Count(), "Setup: debe existir 1 clúster.");

        
        cat.QuitarItemDeCluster(b);
        
        Assert.AreEqual(0, cat.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemSinCluster_NoCambiaNada()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1"  };
        cat.AgregarItem(a);

        cat.QuitarItemDeCluster(a);
        
        Assert.AreEqual(0, cat.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemNull_LanzaArgumentNullException()
    {
        var cat = new Catalogo("Stock Tata");

        var ex = Assert.ThrowsException<ArgumentNullException>(() => cat.QuitarItemDeCluster(null));
        StringAssert.Contains(ex.Message, "El parámetro no puede ser null");
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ConTresMiembros_QuedaClusterConDos()
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
        
        cat.QuitarItemDeCluster(b);
        
        Assert.AreEqual(1, cat.Clusters.Count());
        var cluster = cat.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, c },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemFueraDelCatalogo_LanzaInvalidOperationException()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d1" };

        cat.AgregarItem(a);
        cat.ConfirmarDuplicado(a, a);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => cat.QuitarItemDeCluster(b));
        StringAssert.Contains(ex.Message, "El item no pertenece al catalogo");
    }
    
    [TestMethod]
    public void ObtenerClusterDe_ItemConCluster_RetornaEseCluster()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d1" };
        cat.AgregarItem(a); 
        cat.AgregarItem(b);

        cat.ConfirmarDuplicado(a, b);

        var cluster = cat.ObtenerClusterDe(a);

        Assert.IsNotNull(cluster);
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void Cluster_Canonico_EsElDeDescripcionMasLarga()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "mucho mas larga" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);

        cat.ConfirmarDuplicado(a, b);
        var cluster = cat.Clusters.First();

        Assert.AreSame(b, cluster.Canonico);
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MarcaEsCanonico_SoloEnElItemCanonico()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion larga" };
        cat.AgregarItem(a);
        cat.AgregarItem(b);
        
        cat.ConfirmarDuplicado(a, b);
        var cluster = cat.Clusters.First();
        
        Assert.AreSame(b, cluster.Canonico);
        Assert.IsTrue(b.EsCanonico);
        Assert.IsFalse(a.EsCanonico);
    }
    
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveNoCanonico_EnClusterDeDos_EliminaCluster_YRestanteEsCanonico()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        cat.AgregarItem(a); cat.AgregarItem(b);

        cat.ConfirmarDuplicado(a, b); 

        cat.QuitarItemDeCluster(b);

        Assert.AreEqual(0, cat.Clusters.Count());
        Assert.IsTrue(a.EsCanonico);
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_AgregaTercerItem_ActualizaBanderasCanonico()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion mas larga" };
        var c = new Item { Titulo = "C", Descripcion = "descripcion media" };
        cat.AgregarItem(a); cat.AgregarItem(b); cat.AgregarItem(c);
        
        cat.ConfirmarDuplicado(a, b);
        var cluster = cat.Clusters.First();
        Assert.AreSame(b, cluster.Canonico);
        Assert.IsTrue(b.EsCanonico);
        Assert.IsFalse(a.EsCanonico);

        cat.ConfirmarDuplicado(c, b);
        
        cluster = cat.Clusters.First();
        
        Assert.AreSame(b, cluster.Canonico);
        Assert.IsTrue(b.EsCanonico);
        Assert.IsFalse(a.EsCanonico);
        Assert.IsFalse(c.EsCanonico);
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveCanonico_EnClusterDeTres_YActualizaCanonico()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        var c = new Item { Titulo = "C", Descripcion = "descripcion media" }; 
        cat.AgregarItem(a); 
        cat.AgregarItem(b);
        cat.AgregarItem(c);

        cat.ConfirmarDuplicado(a, b); 
        cat.ConfirmarDuplicado(c, b);
        
        cat.QuitarItemDeCluster(b);

        Assert.AreEqual(1, cat.Clusters.Count());
        Assert.IsTrue(c.EsCanonico);
        Assert.IsFalse(a.EsCanonico);
    }
    
    [TestMethod]
    public void FusionarCampos_IgnoraVaciosYTrim()
    {
        var cat = new Catalogo("Stock Tata");
        var a = new Item { Titulo="X", Descripcion="Descripcion larga",Categoria="No soy vacio" };
        var b  = new Item { Titulo="Y", Descripcion="corta", Marca="alguna", Modelo="otro",Categoria="No importa si tengo" };

        cat.AgregarItem(a); 
        cat.AgregarItem(b);
        cat.ConfirmarDuplicado(a, b);

        var c = cat.Clusters.First().Canonico;
        Assert.AreEqual("alguna",  c.Marca);
        Assert.AreEqual("otro", c.Modelo);
        Assert.AreEqual("No soy vacio", c.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_TomaMasLargo_YNoPisaSiCanonicoYaTiene()
    {
        var cat = new Catalogo("Stock Tata");

        var a = new Item { Titulo="A", Descripcion="larga", Marca="",     Modelo="",     Categoria="" };
        var b = new Item { Titulo="B", Descripcion="mucho mas larga", Marca="AC",    Modelo="M1",  Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="media",            Marca="ACMECO", Modelo="M123", Categoria="Categoria Larguísima" };

        cat.AgregarItem(a); cat.AgregarItem(b); cat.AgregarItem(c);
        cat.ConfirmarDuplicado(a, b);
        cat.ConfirmarDuplicado(b, c);

        var canon = cat.Clusters.First().Canonico;
        
        Assert.AreEqual("AC", canon.Marca);
        
        Assert.AreEqual("M1", canon.Modelo);
        Assert.AreEqual("X", canon.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_CambiaDeCanonicoYCampos_ConTresItems()
    {
        var cat = new Catalogo("Stock Tata");

        var a = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var b = new Item { Titulo="B", Descripcion="larga", Marca="AC", Modelo="M1",  Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="nuevo mas largooo"};

        cat.AgregarItem(a); 
        cat.AgregarItem(b); 
        cat.AgregarItem(c);
        
        cat.ConfirmarDuplicado(a, b);
        var canon = cat.Clusters.First().Canonico;
        Assert.AreSame(a, canon);
        
        cat.ConfirmarDuplicado(b, c);
        
        var canonDespues = cat.Clusters.First().Canonico;
        Assert.AreSame(c, canonDespues);
        
        Assert.AreEqual("AC", canon.Marca);
        Assert.AreEqual("M1", canon.Modelo);
        Assert.AreEqual("X", canon.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_QuitarCanonico_RecalculaYFusionaConRestantes()
    {
        var cat = new Catalogo("Stock Tata");

        var a = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var b = new Item { Titulo="B", Descripcion="media", Marca="AC",  Modelo="M1",   Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="mediaa", Marca="ACME CORPORATION", Modelo="MODEL-2025" };

        cat.AgregarItem(a); 
        cat.AgregarItem(b); 
        cat.AgregarItem(c);
        
        cat.ConfirmarDuplicado(a, b);
        cat.ConfirmarDuplicado(a, c);

        Assert.AreSame(a, cat.Clusters.First().Canonico);

        cat.QuitarItemDeCluster(a);

        var cluster = cat.Clusters.First();
        
        var nuevo = cluster.Canonico;
        
        Assert.AreSame(c, nuevo);
        
        Assert.AreEqual("ACME CORPORATION", nuevo.Marca);
        Assert.AreEqual("MODEL-2025",       nuevo.Modelo);
        Assert.AreEqual("X", nuevo.Categoria);
    }
    
    [TestMethod]
    public void Merge_MarcaEmpateLongitud_EligeLexicograficoAsc()
    {
        var cat = new Catalogo("X");
        
        var a = new Item { Titulo = "AAAAAA",      Descripcion = "ZZZZ", Marca = "Zeta" };
        var b = new Item { Titulo = "BBBB",        Descripcion = "YYYY", Marca = "Beta" };
        
        var c = new Item { Titulo = "BBBBBBBBBBB", Descripcion = "YYYY" };
    
        cat.AgregarItem(a);
        cat.AgregarItem(b);
        cat.AgregarItem(c);
        
        cat.ConfirmarDuplicado(a, b);
        cat.ConfirmarDuplicado(a, c);
        
        var cluster = cat.Clusters.First();
        var nuevo = cluster.Canonico;
        
        Assert.AreEqual(c, nuevo);
        Assert.AreEqual("Beta", nuevo.Marca);
    }
    
    [TestMethod]
    public void EliminarItem_QuePerteneceACluster_LoQuitaDelCluster()
    {
        var catalogo = new Catalogo("Catálogo Prueba");
        var a = new Item { Titulo = "A", Descripcion = "desc" };
        var b = new Item { Titulo = "B", Descripcion = "desc" };
        catalogo.AgregarItem(a);
        catalogo.AgregarItem(b);
        catalogo.ConfirmarDuplicado(a, b);

        var cluster = catalogo.Clusters.First();
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(a));
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(b));
        
        catalogo.EliminarItem(a);
        
        Assert.IsFalse(catalogo.Items.Contains(a));
        Assert.IsFalse(cluster.PertenecientesCluster.Contains(a));
    }
}