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
    public void ConfirmarDuplicado_CreaUnClusterConDosMiembros_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };

        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        
        _catalogo.ConfirmarClusters(itemA, itemB);

        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());

        var cluster = _catalogo.Clusters.First();
        cantidadEsperada = 2;
        var pertenecientesCluster = cluster.PertenecientesCluster.ToList();
        
        Assert.AreEqual(cantidadEsperada, pertenecientesCluster.Count);
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB },
            pertenecientesCluster);
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemB_Null_ErrorTest()
    {
        var item = new Item { Titulo = "A", Descripcion = "d" };
        _catalogo.AgregarItem(item);
        
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.ConfirmarClusters(item, null!));
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
    }
    [TestMethod]
    public void ConfirmarDuplicado_ItemA_Null_ErrorTest()
    {
        var a = new Item { Titulo = "A", Descripcion = "d" };
        _catalogo.AgregarItem(a);
        
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.ConfirmarClusters(null!, a));
        StringAssert.Contains(ex.Message, "El parametro no puede ser null");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ItemNoPerteneceAlCatalogo_ErrorTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };

        _catalogo.AgregarItem(itemA);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.ConfirmarClusters(itemA, itemB));
        StringAssert.Contains(ex.Message, "Uno o ambos ítems no pertenecen al catalogo");
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_RepiteMismoPar_NoCreaNuevoCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        
        _catalogo.ConfirmarClusters(itemA, itemB);
        
        var cantidadClauster = _catalogo.Clusters.Count();
        
        _catalogo.ConfirmarClusters(itemA, itemB); 
        
        Assert.AreEqual(cantidadClauster, _catalogo.Clusters.Count());
    }
    
    
    [TestMethod]
    public void ConfirmarDuplicado_Transitivo_AgregaTerceroAlMismoCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemB = new Item { Titulo = "B", Descripcion = "d2" };
        var itemC = new Item { Titulo = "C", Descripcion = "d3" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.AgregarItem(itemC);
        
        _catalogo.ConfirmarClusters(itemA, itemB); 
        _catalogo.ConfirmarClusters(itemB, itemC);

        var cantidadEsperada = 1;
        var cluster = _catalogo.Clusters.First();
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB, itemC },
            cluster.PertenecientesCluster.ToList()
        );
    }
    [TestMethod]
    public void ConfirmarDuplicado_Transitivo2_AgregaTerceroAlMismoCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };
        var itemC = new Item { Titulo = "C", Descripcion = "d" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.AgregarItem(itemC);
        
        _catalogo.ConfirmarClusters(itemA, itemB);
        
        var cantidadClauster = _catalogo.Clusters.Count();
        
        _catalogo.ConfirmarClusters(itemC , itemA); 
        
        Assert.AreEqual(cantidadClauster, _catalogo.Clusters.Count());
        
        var cluster = _catalogo.Clusters.First();
        
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB, itemC },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_ConectaDosClusters_DebeUnirlos_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemB = new Item { Titulo = "B", Descripcion = "d2" };
        var itemC = new Item { Titulo = "C", Descripcion = "d3" };
        var itemD = new Item { Titulo = "D", Descripcion = "d4" };

        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.AgregarItem(itemC);
        _catalogo.AgregarItem(itemD);
        
        _catalogo.ConfirmarClusters(itemA, itemB);
        
        _catalogo.ConfirmarClusters(itemC, itemD);
        
        var cantidadEsperada = 2;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        _catalogo.ConfirmarClusters(itemB, itemD);

        cantidadEsperada = 1;
            
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        var clusterUnico = _catalogo.Clusters.First();
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB, itemC, itemD },
            clusterUnico.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoParEnOrdenInverso_NoCreaNuevoCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var itemB = new Item { Titulo = "B", Descripcion = "d" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        
        _catalogo.ConfirmarClusters(itemA, itemB);
        _catalogo.ConfirmarClusters(itemB, itemA);
        
        Assert.AreEqual(1, _catalogo.Clusters.Count());
        
        var cluster = _catalogo.Clusters.First();
        
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void ConfirmarDuplicado_MismoItem_NoCreaCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d" };
        var cantidadEsperada = 0;
        
        _catalogo.AgregarItem(itemA);

        _catalogo.ConfirmarClusters(itemA, itemA);

        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarDeCluster_DejaSinClusterSiQuedaUnSoloItem_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemB = new Item { Titulo = "B", Descripcion = "d2" };
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);

        _catalogo.ConfirmarClusters(itemA, itemB);
        
        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(itemB);

        cantidadEsperada = 0;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemSinCluster_NoCambiaNada_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1"};
        var cantidadEsperada = 0;
        
        _catalogo.AgregarItem(itemA);

        _catalogo.QuitarItemDeCluster(itemA);
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemNull_ErrorTest()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => _catalogo.QuitarItemDeCluster(null!));
        
        StringAssert.Contains(ex.Message, "El parámetro no puede ser null");
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ConTresMiembros_QuedaClusterConDos_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemB = new Item { Titulo = "B", Descripcion = "d2" };
        var itemC = new Item { Titulo = "C", Descripcion = "d3" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.AgregarItem(itemC);

        _catalogo.ConfirmarClusters(itemA, itemB);
        _catalogo.ConfirmarClusters(itemB, itemC);

        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(itemB);
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        var cluster = _catalogo.Clusters.First();
        
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemC },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_ItemFueraDelCatalogo_ErrorTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemQuitar = new Item { Titulo = "B", Descripcion = "d1" };

        _catalogo.AgregarItem(itemA);
        _catalogo.ConfirmarClusters(itemA, itemA);
        
        var ex = Assert.ThrowsException<InvalidOperationException>(() => _catalogo.QuitarItemDeCluster(itemQuitar));
        
        StringAssert.Contains(ex.Message, "El item no pertenece al catalogo");
    }
    
    [TestMethod]
    public void ObtenerClusterDe_ItemConCluster_RetornaEseCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "d1" };
        var itemB = new Item { Titulo = "B", Descripcion = "d1" };
        _catalogo.AgregarItem(itemA); 
        _catalogo.AgregarItem(itemB);

        _catalogo.ConfirmarClusters(itemA, itemB);

        var cluster = _catalogo.ObtenerClusterDe(itemA);

        Assert.IsNotNull(cluster);
        CollectionAssert.AreEquivalent(
            new[] { itemA, itemB },
            cluster.PertenecientesCluster.ToList()
        );
    }
    
    [TestMethod]
    public void Cluster_Canonico_EsElDeDescripcionMasLarga_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "corta" };
        var itemCandidato = new Item { Titulo = "B", Descripcion = "mucho mas larga" };
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemCandidato);

        _catalogo.ConfirmarClusters(itemA, itemCandidato);
        
        var cluster = _catalogo.Clusters.First();
        cluster.FusionarCanonico();
        Assert.AreSame(itemCandidato, cluster.Canonico);
    }
    
    [TestMethod]
    public void ClusterVacio_CanonicoNull_OkTest()
    {
        int idCluster = 1;
        var pertenecientes = new HashSet<Item>();
        var cluster = new Cluster(idCluster, pertenecientes);
        
        cluster.FusionarCanonico();
        
        Assert.IsNull(cluster.Canonico);
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveNoCanonico_EnClusterDeDos_EliminaCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "corta" };
        var itemQuitar = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        _catalogo.AgregarItem(itemA); 
        _catalogo.AgregarItem(itemQuitar);

        _catalogo.ConfirmarClusters(itemA, itemQuitar);
        var cantidadEsperada = 1;
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        
        _catalogo.QuitarItemDeCluster(itemQuitar);
        
        cantidadEsperada = 0;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
    }
    
    [TestMethod]
    public void QuitarItemDeCluster_RemueveCanonico_EnClusterDeTres_YActualizaCanonico_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "corta" };
        var itemPrimerCandidato = new Item { Titulo = "B", Descripcion = "descripcion mas larga" }; 
        var itemSegundoCandidato = new Item { Titulo = "C", Descripcion = "descripcion media" }; 
        
        _catalogo.AgregarItem(itemA); 
        _catalogo.AgregarItem(itemPrimerCandidato);
        _catalogo.AgregarItem(itemSegundoCandidato);

        _catalogo.ConfirmarClusters(itemA, itemPrimerCandidato); 
        _catalogo.ConfirmarClusters(itemSegundoCandidato, itemPrimerCandidato);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        Assert.AreSame(itemPrimerCandidato,cluster.Canonico);
        
        _catalogo.QuitarItemDeCluster(itemPrimerCandidato);
        
        cluster.FusionarCanonico();
        
        var cantidadEsperada = 1;
        
        Assert.AreEqual(cantidadEsperada, _catalogo.Clusters.Count());
        Assert.AreSame(itemSegundoCandidato, cluster.Canonico);
    }
    
    [TestMethod]
    public void FusionarCampos_IgnoraVaciosYTrim_OkTest()
    {
        var itemCandidato = new Item { Titulo="X", Descripcion="Descripcion larga",Categoria="No soy vacio" };
        var itemB  = new Item { Titulo="Y", Descripcion="corta", Marca="alguna", Modelo="otro",Categoria="No importa si tengo" };

        _catalogo.AgregarItem(itemCandidato); 
        _catalogo.AgregarItem(itemB);
        _catalogo.ConfirmarClusters(itemCandidato, itemB);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("alguna",  canonico.Marca);
        Assert.AreEqual("otro", canonico.Modelo);
        Assert.AreEqual("No soy vacio", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_TomaMasLargo_YNoPisaSiCanonicoYaTiene_OkTest()
    {
        var itemA = new Item { Titulo="A", Descripcion="larga", Marca="", Modelo="", Categoria="" };
        var itemCandidato = new Item { Titulo="B", Descripcion="mucho mas larga", Marca="AC", Modelo="M1",  Categoria="X" };
        var itemC = new Item { Titulo="C", Descripcion="media", Marca="ACMECO", Modelo="M123", Categoria="Categoria Larguísima" };

        _catalogo.AgregarItem(itemA); 
        _catalogo.AgregarItem(itemCandidato); 
        _catalogo.AgregarItem(itemC);
        
        _catalogo.ConfirmarClusters(itemA, itemCandidato);
        _catalogo.ConfirmarClusters(itemCandidato, itemC);

        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual("AC", canonico.Marca);
        Assert.AreEqual("M1", canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_CambiaDeCanonicoYCampos_ConTresItems_OkTest()
    {
        var itemPrimerCandidato = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var itemB = new Item { Titulo="B", Descripcion="larga", Marca="AC", Modelo="M1",  Categoria="X" };
        var itemSegundoCandidato = new Item { Titulo="C", Descripcion="nuevo mas largooo"};

        _catalogo.AgregarItem(itemPrimerCandidato); 
        _catalogo.AgregarItem(itemB); 
        _catalogo.AgregarItem(itemSegundoCandidato);
        
        _catalogo.ConfirmarClusters(itemPrimerCandidato, itemB);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.AreSame(itemPrimerCandidato, canonico);
        
        _catalogo.ConfirmarClusters(itemB, itemSegundoCandidato);
        
        cluster.FusionarCanonico();
        
        var canonicoDespues = cluster.Canonico;
        
        Assert.AreSame(itemSegundoCandidato, canonicoDespues);
        Assert.IsNotNull(canonico);
        Assert.AreEqual("AC", canonico.Marca);
        Assert.AreEqual("M1", canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_QuitarCanonico_RecalculaYFusionaConRestantes_OkTest()
    {
        var itemCandidatoIncial = new Item { Titulo="A", Descripcion="mucho mas larga", Marca="", Modelo="", Categoria="" };
        var itemB = new Item { Titulo="B", Descripcion="media", Marca="AC",  Modelo="M1",   Categoria="X" };
        var itemCandiatoFinal = new Item { Titulo="C", Descripcion="mediaa", Marca="ACME CORPORATION", Modelo="MODEL-2025" };

        _catalogo.AgregarItem(itemCandidatoIncial); 
        _catalogo.AgregarItem(itemB); 
        _catalogo.AgregarItem(itemCandiatoFinal);
        
        _catalogo.ConfirmarClusters(itemCandidatoIncial, itemB);
        _catalogo.ConfirmarClusters(itemCandidatoIncial, itemCandiatoFinal);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        Assert.AreSame(itemCandidatoIncial, cluster.Canonico);

        _catalogo.QuitarItemDeCluster(itemCandidatoIncial);
        
        cluster.FusionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreSame(itemCandiatoFinal, canonico);
        Assert.AreEqual("ACME CORPORATION", canonico.Marca);
        Assert.AreEqual("MODEL-2025",       canonico.Modelo);
        Assert.AreEqual("X", canonico.Categoria);
    }
    
    [TestMethod]
    public void FusionarCampos_MarcaEmpateLongitud_EligeLexicograficoAsc_OkTest()
    {
        var itemA = new Item { Titulo = "AAAAAA", Descripcion = "ZZZZ", Marca = "Zeta" };
        var itemB = new Item { Titulo = "BBBB", Descripcion = "YYYY", Marca = "Beta" };
        var itemCandidato = new Item { Titulo = "BBBBBBBBBBB", Descripcion = "YYYY" };
    
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        _catalogo.AgregarItem(itemCandidato);
        
        _catalogo.ConfirmarClusters(itemA, itemB);
        _catalogo.ConfirmarClusters(itemA, itemCandidato);
        
        var cluster = _catalogo.Clusters.First();
        
        cluster.FusionarCanonico();
        
        var canonico = cluster.Canonico;
        
        Assert.IsNotNull(canonico);
        Assert.AreEqual(itemCandidato, canonico);
        Assert.AreEqual("Beta", canonico.Marca);
    }
    
    [TestMethod]
    public void EliminarItem_QuePerteneceACluster_LoQuitaDelCluster_OkTest()
    {
        var itemA = new Item { Titulo = "A", Descripcion = "desc" };
        var itemB = new Item { Titulo = "B", Descripcion = "desc" };
        
        _catalogo.AgregarItem(itemA);
        _catalogo.AgregarItem(itemB);
        
        _catalogo.ConfirmarClusters(itemA, itemB);

        var cluster = _catalogo.Clusters.First();
        
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(itemA));
        Assert.IsTrue(cluster.PertenecientesCluster.Contains(itemB));
        
        _catalogo.EliminarItem(itemA);
        
        Assert.IsFalse(_catalogo.Items.Contains(itemA));
        Assert.IsFalse(cluster.PertenecientesCluster.Contains(itemA));
    }
}