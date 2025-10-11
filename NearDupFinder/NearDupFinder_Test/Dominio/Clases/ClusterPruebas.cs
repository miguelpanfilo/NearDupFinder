using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class ClusterPruebas
{
    private Catalogo _catalogo = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _catalogo = new Catalogo("X");
    }
    

    [TestMethod]
    public void ConfirmarDuplicado_CreaUnClusterConDosMiembros()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };

        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        
        _catalogo.ConfirmarClusters(a, b);

        Assert.AreEqual(1, _catalogo.Clusters.Count());

        var cluster = _catalogo.Clusters.First();
        
        var pertenecientesCluster = cluster.PertenecientesCluster.ToList();
        Assert.AreEqual(2, pertenecientesCluster.Count);
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            pertenecientesCluster);
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemB_Null_LanzaArgumentNullException()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        _catalogo.AgregarItem(a);
        
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.ConfirmarClusters(a, null!));
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
    }
    [TestMethod]
    public void ConfirmarDuplicado_ItemA_Null_LanzaArgumentNullException()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        _catalogo.AgregarItem(a);
        
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.ConfirmarClusters(null!, a));
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemNoPerteneceAlCatalogo_LanzaInvalidOperationException()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };

        _catalogo.AgregarItem(a);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.ConfirmarClusters(a, b));
        StringAssert.Contains(ex.Message, "Uno o ambos ítems no pertenecen al catalogo");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_RepiteMismoPar_NoCreaNuevoCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        
        _catalogo.ConfirmarClusters(a, b);
        var cantidadClauster = _catalogo.Clusters.Count();
        _catalogo.ConfirmarClusters(a, b); 
        
        Assert.AreEqual(cantidadClauster, _catalogo.Clusters.Count());
    }
    
    
    [TestMethod]
    public void ConfirmarDuplicado_Transitivo_AgregaTerceroAlMismoCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        var c = new Item { Titulo = "C", Descripcion = "d3" };
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b); 
        _catalogo.ConfirmarClusters(b, c); 
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        var cluster = _catalogo.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b, c },
            cluster.PertenecientesCluster.ToList()
        );
    }
    [TestMethod]
    public void ConfirmarDuplicado_Transitivo2_AgregaTerceroAlMismoCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };
        var c = new Item { Titulo = "C", Descripcion = "d" };
        
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b);
        
        var cantidadClauster = _catalogo.Clusters.Count();
        
        _catalogo.ConfirmarClusters(c , a); 
        
        Assert.AreEqual(cantidadClauster, _catalogo.Clusters.Count());
        
        var cluster = _catalogo.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b, c },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ConectaDosClusters_DebeUnirlos()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        var c = new Item { Titulo = "C", Descripcion = "d3" };
        var d = new Item { Titulo = "D", Descripcion = "d4" };

        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);
        _catalogo.AgregarItem(d);
        
        _catalogo.ConfirmarClusters(a, b);
        
        _catalogo.ConfirmarClusters(c, d);
        
        Assert.AreEqual(2, _catalogo.Clusters.Count());
        
        _catalogo.ConfirmarClusters(b, d);
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        var clusterUnico = _catalogo.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, b, c, d },
            clusterUnico.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoParEnOrdenInverso_NoCreaNuevoCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        var b = new Item { Titulo = "B", Descripcion = "d" };
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        
        _catalogo.ConfirmarClusters(a, b);
        _catalogo.ConfirmarClusters(b, a);
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        var cluster = _catalogo.Clusters.First();
        
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoItem_NoCreaCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        _catalogo.AgregarItem(a);

        _catalogo.ConfirmarClusters(a, a);

        Assert.AreEqual(0, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarDeCluster_DejaSinClusterSiQuedaUnSoloItem()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);

        _catalogo.ConfirmarClusters(a, b);
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(b);
        
        Assert.AreEqual(0, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemSinCluster_NoCambiaNada()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1"};
        
        _catalogo.AgregarItem(a);

        _catalogo.QuitarItemDeCluster(a);
        
        Assert.AreEqual(0, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemNull_LanzaArgumentNullException()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.QuitarItemDeCluster(null!));
        
        StringAssert.Contains(ex.Message, "El parámetro no puede ser null");
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ConTresMiembros_QuedaClusterConDos()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d2" };
        var c = new Item { Titulo = "C", Descripcion = "d3" };
        
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);

        _catalogo.ConfirmarClusters(a, b);
        _catalogo.ConfirmarClusters(b, c);

        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(b);
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        var cluster = _catalogo.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { a, c },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemFueraDelCatalogo_LanzaInvalidOperationException()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d1" };

        _catalogo.AgregarItem(a);
        _catalogo.ConfirmarClusters(a, a);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.QuitarItemDeCluster(b));
        
        StringAssert.Contains(ex.Message, "El item no pertenece al catalogo");
    }
    
    [TestMethod]
    public void ObtenerClusterDe_ItemConCluster_RetornaEseCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "d1" };
        var b = new Item { Titulo = "B", Descripcion = "d1" };
        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b);

        _catalogo.ConfirmarClusters(a, b);

        var cluster = _catalogo.ObtenerClusterDe(a);

        Assert.IsNotNull(cluster);
        CollectionAssert.AreEquivalent(
            new[] { a, b },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void Cluster_Canonico_EsElDeDescripcionMasLarga()
    {
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "mucho mas larga" };
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);

        _catalogo.ConfirmarClusters(a, b);
        
        var cluster = _catalogo.Clusters.First();
        cluster.FuncionarCanonico();
        Assert.AreSame(b, cluster.Canonico);
    }
    
    [TestMethod]
    public void ClusterVacio_CanonicoNull()
    {
        int idCluster = 1;
        var pertenecientes = new HashSet<Item>();
        var cluster = new Cluster(idCluster, pertenecientes);
        
        cluster.FuncionarCanonico();
        
        Assert.IsNull(cluster.Canonico);
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveNoCanonico_EnClusterDeDos_EliminaCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b);

        _catalogo.ConfirmarClusters(a, b); 
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(b);

        Assert.AreEqual(0, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveCanonico_EnClusterDeTres_YActualizaCanonico()
    {
        var a = new Item { Titulo = "A", Descripcion = "corta" };
        var b = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        var c = new Item { Titulo = "C", Descripcion = "descripcion media" }; 
        
        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);

        _catalogo.ConfirmarClusters(a, b); 
        _catalogo.ConfirmarClusters(c, b);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        
        Assert.AreSame(b,cluster.Canonico);
        
        _catalogo.QuitarItemDeCluster(b);
        
        cluster.FuncionarCanonico();
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        Assert.AreSame(c, cluster.Canonico);
    }
    
    [TestMethod]
    public void FusionarCampos_IgnoraVaciosYTrim()
    {
        var a = new Item { Titulo="X", Descripcion="Descripcion larga",Categoria="No soy vacio" };
        var b  = new Item { Titulo="Y", Descripcion="corta", Marca="alguna", Modelo="otro",Categoria="No importa si tengo" };

        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b);
        _catalogo.ConfirmarClusters(a, b);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("alguna",  canonico.Marca);
        Assert.AreEqual("otro", canonico.Modelo);
        Assert.AreEqual("No soy vacio", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_TomaMasLargo_YNoPisaSiCanonicoYaTiene()
    {
        var a = new Item { Titulo="A", Descripcion="larga", Marca="",     Modelo="",     Categoria="" };
        var b = new Item { Titulo="B", Descripcion="mucho mas larga", Marca="AC",    Modelo="M1",  Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="media",            Marca="ACMECO", Modelo="M123", Categoria="Categoria Larguísima" };

        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b); 
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b);
        _catalogo.ConfirmarClusters(b, c);

        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("AC", canonico.Marca);
        Assert.AreEqual("M1", canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_CambiaDeCanonicoYCampos_ConTresItems()
    {
        var a = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var b = new Item { Titulo="B", Descripcion="larga", Marca="AC", Modelo="M1",  Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="nuevo mas largooo"};

        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b); 
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b);
        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        var canonico = cluster.Canonico;
        
        Assert.AreSame(a, canonico);
        
        _catalogo.ConfirmarClusters(b, c);
        cluster.FuncionarCanonico();
        
        var canonicoDespues = cluster.Canonico;
        
        Assert.AreSame(c, canonicoDespues);
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("AC", canonico.Marca);
        Assert.AreEqual("M1", canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_QuitarCanonico_RecalculaYFusionaConRestantes()
    {
        var a = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var b = new Item { Titulo="B", Descripcion="media", Marca="AC",  Modelo="M1",   Categoria="X" };
        var c = new Item { Titulo="C", Descripcion="mediaa", Marca="ACME CORPORATION", Modelo="MODEL-2025" };

        _catalogo.AgregarItem(a); 
        _catalogo.AgregarItem(b); 
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b);
        _catalogo.ConfirmarClusters(a, c);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        
        Assert.AreSame(a, cluster.Canonico);

        _catalogo.QuitarItemDeCluster(a);
        
        cluster.FuncionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreSame(c, canonico);
        Assert.AreEqual("ACME CORPORATION", canonico.Marca);
        Assert.AreEqual("MODEL-2025",       canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_MarcaEmpateLongitud_EligeLexicograficoAsc()
    {
        var a = new Item { Titulo = "AAAAAA",      Descripcion = "ZZZZ", Marca = "Zeta" };
        var b = new Item { Titulo = "BBBB",        Descripcion = "YYYY", Marca = "Beta" };
        
        var c = new Item { Titulo = "BBBBBBBBBBB", Descripcion = "YYYY" };
    
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        _catalogo.AgregarItem(c);
        
        _catalogo.ConfirmarClusters(a, b);
        _catalogo.ConfirmarClusters(a, c);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FuncionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual(c, canonico);
        Assert.AreEqual("Beta", canonico.Marca);
    }
    
    [TestMethod]
    public void EliminarItem_QuePerteneceACluster_LoQuitaDelCluster()
    {
        var a = new Item { Titulo = "A", Descripcion = "desc" };
        var b = new Item { Titulo = "B", Descripcion = "desc" };
        
        _catalogo.AgregarItem(a);
        _catalogo.AgregarItem(b);
        
        _catalogo.ConfirmarClusters(a, b);

        var cluster = _catalogo.Clusters.First();
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(a));
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(b));
        
        _catalogo.EliminarItem(a);
        
        Assert.IsFalse(_catalogo.Items.Contains(a));
        Assert.IsFalse(cluster.PertenecientesCluster.Contains(a));
    }
}