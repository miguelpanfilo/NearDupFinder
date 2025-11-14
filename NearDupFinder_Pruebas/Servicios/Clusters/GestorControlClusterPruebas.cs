using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Clusters
{
    [TestClass]
    public class GestorControlClusterPruebas
    {
        private SqlContext _context = null!;
        private GestorCatalogos _gestorCatalogos = null!;
        private GestorAuditoria _gestorAuditoria = null!;
        private GestorControlClusters _gestorControlClusters = null!;
        private Catalogo _catalogo = null!;

        [TestInitialize]
        public void Setup()
        {
            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory($"BD_Clusters_{Guid.NewGuid()}");
            _context = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

            Item.ResetearContadorId();

            IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);
            IRepositorioClusters repoClusters = new RepositorioClusters(_context);
            IRepositorioItems repoItems = new RepositorioItems(_context);
            IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context);

            var sesionUsuario = new SesionUsuarioActual();
            sesionUsuario.Asignar("tester@correo.com");

            _gestorAuditoria = new GestorAuditoria(repoAuditorias, sesionUsuario);
            _gestorCatalogos = new GestorCatalogos(repoCatalogos);

            _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Catálogo de Prueba"));
            _catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Catálogo de Prueba")!;

            _gestorControlClusters = new GestorControlClusters(
                _gestorCatalogos,
                _gestorAuditoria,
                repoCatalogos,
                repoClusters,
                repoItems
            );
        }
      
        private void Guardar() => _context.SaveChanges();
        private void RefrescarCatalogo() => _catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Catálogo de Prueba")!;

        [TestMethod]
        public void ConfirmarCluster_MismoItem_RetornaFalseYNoCreaClusters_OkTest()
        {
            var item = new Item { Titulo = "Único", Descripcion = "desc" };
            _catalogo.AgregarItem(item);
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosDuplicados(_catalogo.Id, item.Id, item.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            Assert.IsFalse(ok);
            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ConfirmarCluster_ItemsYaEstanEnMismoCluster_RetornaTrueSinCambios_OkTest()
        {
            Item a = Item.Crear("A", "d");
            Item b = Item.Crear("B", "d");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            _catalogo.CrearCluster(new HashSet<Item> { a, b });
            Guardar();
            RefrescarCatalogo();
            var cantidadInicial = _catalogo.Clusters.Count;
            var datos = new DatosDuplicados(_catalogo.Id, a.Id, b.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            
            Assert.IsTrue(ok);
            Assert.AreEqual(cantidadInicial, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ConfirmarCluster_ItemAEstaEnClusterYItemBNo_AgregaBAlClusterDeA_OkTest()
        {
            Item a = Item.Crear("A", "d");
            Item b = Item.Crear("B", "d");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            _catalogo.CrearCluster(new HashSet<Item> { a });
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosDuplicados(_catalogo.Id, a.Id, b.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            var clusterA = _catalogo.ObtenerClusterDe(a);

            Assert.IsTrue(ok);
            Assert.IsNotNull(clusterA);
            CollectionAssert.AreEquivalent(new[] { a.Id, b.Id }, clusterA!.PertenecientesCluster.Select(i => i.Id).ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_ItemBEstaEnClusterYItemANo_AgregaAAlClusterDeB_OkTest()
        {
            Item a = Item.Crear("A", "d");
            Item b = Item.Crear("B", "d");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            _catalogo.CrearCluster(new HashSet<Item> { b });
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosDuplicados(_catalogo.Id, a.Id, b.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            var clusterB = _catalogo.ObtenerClusterDe(b);

            Assert.IsTrue(ok);
            Assert.IsNotNull(clusterB);
            CollectionAssert.AreEquivalent(new[] { a.Id, b.Id }, clusterB!.PertenecientesCluster.Select(i => i.Id).ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_ItemsEnClustersDistintos_FusionaAmbosEnUnoSolo_OkTest()
        {
            Item a1 = Item.Crear("A1", "d");
            Item a2 = Item.Crear("A2", "d");
            Item b1 = Item.Crear("B1", "d");
            Item b2 = Item.Crear("B2", "d");

            _catalogo.AgregarItem(a1);
            _catalogo.AgregarItem(a2);
            _catalogo.AgregarItem(b1);
            _catalogo.AgregarItem(b2);
            _catalogo.CrearCluster(new HashSet<Item> { a1, a2 });
            _catalogo.CrearCluster(new HashSet<Item> { b1, b2 });
            Guardar();
            RefrescarCatalogo();
            var cantInicial = _catalogo.Clusters.Count;
            var datos = new DatosDuplicados(_catalogo.Id, a1.Id, b2.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            var clusterUnico = _catalogo.Clusters.First();
            
            Assert.IsTrue(ok);
            Assert.AreEqual(cantInicial - 1, _catalogo.Clusters.Count);
            CollectionAssert.AreEquivalent(new[] { a1.Id, a2.Id, b1.Id, b2.Id }, clusterUnico.PertenecientesCluster.Select(i => i.Id).ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_NingunoEstaEnCluster_CreaClusterConAmbos_OkTest()
        {
            Item a = Item.Crear("A", "d");
            Item b = Item.Crear("B", "d");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosDuplicados(_catalogo.Id, a.Id, b.Id);
            var ok = _gestorControlClusters.ConfirmarCluster(datos);
            RefrescarCatalogo();
            var cluster = _catalogo.Clusters.First();

            Assert.IsTrue(ok);
            Assert.AreEqual(1, _catalogo.Clusters.Count);
            CollectionAssert.AreEquivalent(new[] { a.Id, b.Id }, cluster.PertenecientesCluster.Select(i => i.Id).ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_CatalogoInexistente_LanzaExcepcionCatalogo_OkTest()
        {
            var datos = new DatosDuplicados(999999, 1, 2);
            Assert.ThrowsException<ExcepcionCatalogo>(() => _gestorControlClusters.ConfirmarCluster(datos));
        }

        [TestMethod]
        public void ConfirmarCluster_ItemInexistenteEnCatalogo_LanzaExcepcionItem_OkTest()
        {
            var a = new Item { Titulo = "A", Descripcion = "d" };
            _catalogo.AgregarItem(a);
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosDuplicados(_catalogo.Id, a.Id, 9999);
            Assert.ThrowsException<ExcepcionItem>(() => _gestorControlClusters.ConfirmarCluster(datos));
        }

        [TestMethod]
        public void BorrarItemDelCluster_ItemPerteneceACluster_EliminaItemDelClusterYSiQuedaBajoMinimoEliminaCluster_OkTest()
        {
            Item a = Item.Crear("A", "d");
            Item b = Item.Crear("B", "d");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            _catalogo.CrearCluster(new HashSet<Item> { a, b });
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosRemoverItemCluster(b.Id, _catalogo.Id);
            _gestorControlClusters.BorrarItemDelCluster(datos);
            RefrescarCatalogo();
            
            Assert.AreEqual(0, _catalogo.Clusters.Count);
            Assert.IsNotNull(_catalogo.ObtenerItemPorId(b.Id));
        }

        [TestMethod]
        public void BorrarItemDelCluster_ItemRemovidoEraCanonico_NulificaCanonicoSiElClusterSigueExistiendo_OkTest()
        {
            Item canonico = Item.Crear("Candidato", "descripcion mucho mas larga", "AC");
            Item otro = Item.Crear("Otro", "descripcion");
            Item tercero = Item.Crear("Tercero", "desc");
            
            _catalogo.AgregarItem(canonico);
            _catalogo.AgregarItem(otro);
            _catalogo.AgregarItem(tercero);
            _catalogo.CrearCluster(new HashSet<Item> { canonico, otro, tercero });
            Guardar();
            RefrescarCatalogo();
            var cluster = _catalogo.Clusters.First();
            cluster.FusionarCanonico();
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosRemoverItemCluster(canonico.Id, _catalogo.Id);
            _gestorControlClusters.BorrarItemDelCluster(datos);
            RefrescarCatalogo();
            
            Assert.AreEqual(1, _catalogo.Clusters.Count);
            Assert.IsNull(cluster.Canonico);
        }

        [TestMethod]
        public void BorrarItemDelCluster_ItemSinCluster_NoRealizaCambios_OkTest()
        {
            Item a = Item.Crear("A", "d");
            
            _catalogo.AgregarItem(a);
            Guardar();
            RefrescarCatalogo();
            var datos = new DatosRemoverItemCluster(a.Id, _catalogo.Id);
            _gestorControlClusters.BorrarItemDelCluster(datos);
            RefrescarCatalogo();
            
            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void FusionarItemsEnElCluster_SinCanonicoPrevio_AsignaCanonicoSegunReglas_OkTest()
        {
            Item largo = Item.Crear("A", "descripcion muy larga");
            Item corto = Item.Crear("A", "corta");

            _catalogo.AgregarItem(largo);
            _catalogo.AgregarItem(corto);
            _catalogo.CrearCluster(new HashSet<Item> { largo, corto });
            Guardar();
            RefrescarCatalogo();
            var cluster = _catalogo.Clusters.First();
            _gestorControlClusters.FusionarItemsEnElCluster(new DatosFusionarItems(_catalogo.Id, cluster.Id));
            RefrescarCatalogo();
            var clusterRefrescado = _catalogo.Clusters.First();
            
            Assert.IsNotNull(clusterRefrescado.Canonico);
            Assert.AreEqual(largo.Id, clusterRefrescado.Canonico!.Id);
        }

        [TestMethod]
        public void FusionarItemsEnElCluster_ClusterInexistente_LanzaExcepcionCatalogo_YNoRealizaCambios_OkTest()
        {
            var cantInicial = _catalogo.Clusters.Count;
            var datos = new DatosFusionarItems(_catalogo.Id, 987654321);

            var ex = Assert.ThrowsException<ExcepcionCatalogo>(
                () => _gestorControlClusters.FusionarItemsEnElCluster(datos)
            );

            Assert.AreEqual("Cluster inexistente.", ex.Message);
            RefrescarCatalogo();
            Assert.AreEqual(cantInicial, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ItemEstaEnCluster_ItemPerteneceAAlguno_ReturnsTrue_OkTest()
        {
            var a = new Item { Titulo = "A", Descripcion = "d" };
            _catalogo.AgregarItem(a);
            _catalogo.CrearCluster(new HashSet<Item> { a });
            Guardar();
            RefrescarCatalogo();
            var ok = _gestorControlClusters.ItemEstaEnCluster(_catalogo.Id, a.Id);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void ItemEstaEnCluster_ItemNoPerteneceANinguno_ReturnsFalse_OkTest()
        {
            var a = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(a);
            Guardar();
            RefrescarCatalogo();
            var ok = _gestorControlClusters.ItemEstaEnCluster(_catalogo.Id, a.Id);
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_SinClusters_RetornaColeccionVacia_OkTest()
        {
            var clusters = _gestorControlClusters.ObtenerClustersDtoCatalogo(_catalogo.Id);
            Assert.IsNotNull(clusters);
            Assert.AreEqual(0, clusters.Count);
            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_ConClustersExistentes_RetornaCantidadEIdsCorrectos_OkTest()
        {
            Item a = Item.Crear("A", "desc A");
            Item b = Item.Crear("B", "desc B");
            Item c = Item.Crear("C", "desc C");
            Item d = Item.Crear("D", "desc D");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            _catalogo.AgregarItem(c);
            _catalogo.AgregarItem(d);
            Guardar();
            RefrescarCatalogo();
            var dupAB = new DatosDuplicados(_catalogo.Id, a.Id, b.Id);
            var dupCD = new DatosDuplicados(_catalogo.Id, c.Id, d.Id);
            var r1 = _gestorControlClusters.ConfirmarCluster(dupAB);
            var r2 = _gestorControlClusters.ConfirmarCluster(dupCD);
            RefrescarCatalogo();
            var idsDom = _catalogo.Clusters.Select(cu => cu.Id).OrderBy(x => x).ToArray();
            var dtos = _gestorControlClusters.ObtenerClustersDtoCatalogo(_catalogo.Id);
            var idsDto = dtos.Select(d => d.Id).OrderBy(x => x).ToArray();

            Assert.IsTrue(r1);
            Assert.IsTrue(r2);
            Assert.AreEqual(2, _catalogo.Clusters.Count);
            Assert.AreEqual(2, dtos.Count);
            CollectionAssert.AreEqual(idsDom, idsDto);
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_CatalogoInexistente_LanzaExcepcionCatalogo_OkTest()
        {
            var ex = Assert.ThrowsException<ExcepcionCatalogo>(() => _gestorControlClusters.ObtenerClustersDtoCatalogo(9999));
            Assert.AreEqual("Catálogo no encontrado (Id=9999).", ex.Message);
        }

        [TestMethod]
        public void DatosPublicosCluster_Id_MapeaIgual_OkTest()
        {
            Item a = Item.Crear("A", "desc A");
            Item b = Item.Crear("B", "desc B");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            Guardar();
            RefrescarCatalogo();
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, a.Id, b.Id));
            RefrescarCatalogo();
            var cluster = _catalogo.Clusters.First();
            var dto = DatosPublicosCluster.FromEntity(cluster);
            
            Assert.AreEqual(cluster.Id, dto.Id);
        }

        [TestMethod]
        public void DatosPublicosCluster_Pertenecientes_CantidadCorrecta_OkTest()
        {
            Item a = Item.Crear("A", "desc A");
            Item b = Item.Crear("B", "desc B");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            Guardar();
            RefrescarCatalogo();
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, a.Id, b.Id));
            RefrescarCatalogo();
            var dto = DatosPublicosCluster.FromEntity(_catalogo.Clusters.First());
            
            Assert.AreEqual(2, dto.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void DatosPublicosCluster_Canonico_NoEsNulo_OkTest()
        {
            Item a = Item.Crear("A", "desc A bien larga");
            Item b = Item.Crear("B", "desc B");
            
            _catalogo.AgregarItem(a);
            _catalogo.AgregarItem(b);
            Guardar();
            RefrescarCatalogo();
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, a.Id, b.Id));
            RefrescarCatalogo();
            var cluster = _catalogo.Clusters.First();
            _gestorControlClusters.FusionarItemsEnElCluster(new DatosFusionarItems(_catalogo.Id, cluster.Id));
            RefrescarCatalogo();
            var dto = DatosPublicosCluster.FromEntity(_catalogo.Clusters.First());
            
            Assert.IsNotNull(dto.Canonico);
        }
        
        [TestMethod]
        public void ReservarStockEnCluster_ReservaDesdeMenorStock_PersisteCambios_OkTest()
        {
            Item i1 = Item.Crear("A", "d1");
            i1.Stock = 1;
            Item i2 = Item.Crear("B", "d1");
            i2.Stock = 3;
            Item i3 = Item.Crear("C", "d1");
            i3.Stock = 7;

            _catalogo.AgregarItem(i1);
            _catalogo.AgregarItem(i2);
            _catalogo.AgregarItem(i3);
            _catalogo.CrearCluster(new HashSet<Item> { i1, i2, i3 });
            Guardar();
            RefrescarCatalogo();

            var cluster = _catalogo.Clusters.First();
            cluster.FusionarCanonico();
            Guardar();
            RefrescarCatalogo();

            var cantidad = 4;
            _gestorControlClusters.ReservarStockEnCluster(_catalogo.Id, cluster.Id, cantidad);
            RefrescarCatalogo();

            var clusterRef = _catalogo.Clusters.First();
            var byId = clusterRef.PertenecientesCluster.ToDictionary(x => x.Titulo, x => x.Stock);
            
            Assert.AreEqual(0, byId["A"]);
            Assert.AreEqual(0, byId["B"]);
            Assert.AreEqual(7, byId["C"]);
            Assert.AreEqual(7, clusterRef.StockActual);
        }
        
        [TestMethod]
        public void ReservarStockEnCluster_ClusterInexistente_LanzaExcepcionCatalogo_OkTest()
        {
            var ex = Assert.ThrowsException<ExcepcionCatalogo>(() =>
                _gestorControlClusters.ReservarStockEnCluster(_catalogo.Id, 123456789, 1)
            );
            Assert.AreEqual("Cluster inexistente.", ex.Message);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-10)]
        public void ReservarStockEnCluster_CantidadInvalida_LanzaInvalidOperationException_OkTest(int cantidadInvalida)
        {
            Item i1 = Item.Crear("A", "d1");
            i1.Stock = 5;
            Item i2 = Item.Crear("B", "d1");
            i1.Stock = 5;
            
            _catalogo.AgregarItem(i1);
            _catalogo.AgregarItem(i2);
            _catalogo.CrearCluster(new HashSet<Item> { i1, i2 });
            Guardar();
            RefrescarCatalogo();

            var cluster = _catalogo.Clusters.First();

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _gestorControlClusters.ReservarStockEnCluster(_catalogo.Id, cluster.Id, cantidadInvalida)
            );
            Assert.IsTrue(ex.Message.ToLower().Contains("cantidad inválida"));
        }
    }
}
