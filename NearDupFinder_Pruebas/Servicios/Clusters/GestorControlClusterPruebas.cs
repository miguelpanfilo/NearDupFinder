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
        public void ConfigurarEscenarioDePruebas()
        {
            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Clusters");
            _context = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

            IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);

            _gestorAuditoria = new GestorAuditoria();
            _gestorCatalogos = new GestorCatalogos(repoCatalogos);

            _gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Catálogo de Prueba"));
            _catalogo = _gestorCatalogos.ObtenerCatalogoPorTitulo("Catálogo de Prueba")!;

            _gestorControlClusters = new GestorControlClusters(_gestorCatalogos, _gestorAuditoria);
        }
    

        [TestMethod]
        public void ConfirmarCluster_MismoItem_RetornaFalseYNoCreaClusters_OkTest()
        {
            var itemUnicoConDescripcionDetallada = new Item { Titulo = "Único", Descripcion = "desc" };
            _catalogo.AgregarItem(itemUnicoConDescripcionDetallada);

            var datosDuplicadosQueApuntanAlMismoItem = new DatosDuplicados(
                _catalogo.Id,
                itemUnicoConDescripcionDetallada.Id,
                itemUnicoConDescripcionDetallada.Id);

            var resultadoOperacionDeConfirmacion =
                _gestorControlClusters.ConfirmarCluster(datosDuplicadosQueApuntanAlMismoItem);

            Assert.IsFalse(resultadoOperacionDeConfirmacion);
            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ConfirmarCluster_ItemsYaEstanEnMismoCluster_RetornaTrueSinCambios_OkTest()
        {
            var itemPrimeroYaEnCluster = new Item { Titulo = "A", Descripcion = "d" };
            var itemSegundoYaEnCluster = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemPrimeroYaEnCluster);
            _catalogo.AgregarItem(itemSegundoYaEnCluster);

            _catalogo.CrearCluster(new HashSet<Item> { itemPrimeroYaEnCluster, itemSegundoYaEnCluster });
            var cantidadInicialDeClustersAntes = _catalogo.Clusters.Count;

            var datosDuplicadosReconfirmacion = new DatosDuplicados(
                _catalogo.Id,
                itemPrimeroYaEnCluster.Id,
                itemSegundoYaEnCluster.Id);

            var resultadoOperacionDeConfirmacion =
                _gestorControlClusters.ConfirmarCluster(datosDuplicadosReconfirmacion);

            Assert.IsTrue(resultadoOperacionDeConfirmacion);
            Assert.AreEqual(cantidadInicialDeClustersAntes, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ConfirmarCluster_ItemAEstaEnClusterYItemBNo_AgregaBAlClusterDeA_OkTest()
        {
            var itemBaseQueYaPerteneceAUnCluster = new Item { Titulo = "A", Descripcion = "d" };
            var itemQueDebeSerAgregadoAlCluster = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemBaseQueYaPerteneceAUnCluster);
            _catalogo.AgregarItem(itemQueDebeSerAgregadoAlCluster);

            _catalogo.CrearCluster(new HashSet<Item> { itemBaseQueYaPerteneceAUnCluster });

            var datosDuplicadosParaAgregar = new DatosDuplicados(
                _catalogo.Id,
                itemBaseQueYaPerteneceAUnCluster.Id,
                itemQueDebeSerAgregadoAlCluster.Id);

            var resultadoOperacionDeConfirmacion = _gestorControlClusters.ConfirmarCluster(datosDuplicadosParaAgregar);

            Assert.IsTrue(resultadoOperacionDeConfirmacion);

            var clusterDestinoLuegoDeConfirmacion = _catalogo.ObtenerClusterDe(itemBaseQueYaPerteneceAUnCluster);
            Assert.IsNotNull(clusterDestinoLuegoDeConfirmacion);
            CollectionAssert.AreEquivalent(
                new[] { itemBaseQueYaPerteneceAUnCluster, itemQueDebeSerAgregadoAlCluster },
                clusterDestinoLuegoDeConfirmacion!.PertenecientesCluster.ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_ItemBEstaEnClusterYItemANo_AgregaAAlClusterDeB_OkTest()
        {
            var itemQueDebeSerAgregadoAlClusterDeB = new Item { Titulo = "A", Descripcion = "d" };
            var itemBaseQueDefineElClusterB = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemQueDebeSerAgregadoAlClusterDeB);
            _catalogo.AgregarItem(itemBaseQueDefineElClusterB);

            _catalogo.CrearCluster(new HashSet<Item> { itemBaseQueDefineElClusterB });

            var datosDuplicadosParaAgregar = new DatosDuplicados(
                _catalogo.Id,
                itemQueDebeSerAgregadoAlClusterDeB.Id,
                itemBaseQueDefineElClusterB.Id);

            var resultadoOperacionDeConfirmacion = _gestorControlClusters.ConfirmarCluster(datosDuplicadosParaAgregar);

            Assert.IsTrue(resultadoOperacionDeConfirmacion);

            var clusterDestinoLuegoDeConfirmacion = _catalogo.ObtenerClusterDe(itemBaseQueDefineElClusterB);
            Assert.IsNotNull(clusterDestinoLuegoDeConfirmacion);
            CollectionAssert.AreEquivalent(
                new[] { itemBaseQueDefineElClusterB, itemQueDebeSerAgregadoAlClusterDeB },
                clusterDestinoLuegoDeConfirmacion!.PertenecientesCluster.ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_ItemsEnClustersDistintos_FusionaAmbosEnUnoSolo_OkTest()
        {
            var itemDelClusterA_Primero = new Item { Titulo = "A1", Descripcion = "d" };
            var itemDelClusterA_Segundo = new Item { Titulo = "A2", Descripcion = "d" };
            var itemDelClusterB_Primero = new Item { Titulo = "B1", Descripcion = "d" };
            var itemDelClusterB_Segundo = new Item { Titulo = "B2", Descripcion = "d" };
            _catalogo.AgregarItem(itemDelClusterA_Primero);
            _catalogo.AgregarItem(itemDelClusterA_Segundo);
            _catalogo.AgregarItem(itemDelClusterB_Primero);
            _catalogo.AgregarItem(itemDelClusterB_Segundo);

            _catalogo.CrearCluster(new HashSet<Item> { itemDelClusterA_Primero, itemDelClusterA_Segundo });
            _catalogo.CrearCluster(new HashSet<Item> { itemDelClusterB_Primero, itemDelClusterB_Segundo });

            var cantidadInicialDeClustersAntesDeUnificar = _catalogo.Clusters.Count;

            var datosDuplicadosParaUnificar = new DatosDuplicados(
                _catalogo.Id,
                itemDelClusterA_Primero.Id,
                itemDelClusterB_Segundo.Id);

            var resultadoOperacionDeConfirmacion = _gestorControlClusters.ConfirmarCluster(datosDuplicadosParaUnificar);

            Assert.IsTrue(resultadoOperacionDeConfirmacion);
            Assert.AreEqual(cantidadInicialDeClustersAntesDeUnificar - 1, _catalogo.Clusters.Count);

            var clusterUnificado = _catalogo.Clusters.First();
            CollectionAssert.AreEquivalent(
                new[]
                {
                    itemDelClusterA_Primero, itemDelClusterA_Segundo, itemDelClusterB_Primero, itemDelClusterB_Segundo
                },
                clusterUnificado.PertenecientesCluster.ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_NingunoEstaEnCluster_CreaClusterConAmbos_OkTest()
        {
            var itemParaCrearCluster_Primero = new Item { Titulo = "A", Descripcion = "d" };
            var itemParaCrearCluster_Segundo = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemParaCrearCluster_Primero);
            _catalogo.AgregarItem(itemParaCrearCluster_Segundo);

            var datosDuplicadosParaCrear = new DatosDuplicados(
                _catalogo.Id,
                itemParaCrearCluster_Primero.Id,
                itemParaCrearCluster_Segundo.Id);

            var resultadoOperacionDeConfirmacion = _gestorControlClusters.ConfirmarCluster(datosDuplicadosParaCrear);

            Assert.IsTrue(resultadoOperacionDeConfirmacion);
            Assert.AreEqual(1, _catalogo.Clusters.Count);

            var clusterRecienCreado = _catalogo.Clusters.First();
            CollectionAssert.AreEquivalent(
                new[] { itemParaCrearCluster_Primero, itemParaCrearCluster_Segundo },
                clusterRecienCreado.PertenecientesCluster.ToList());
        }

        [TestMethod]
        public void ConfirmarCluster_CatalogoInexistente_LanzaExcepcionCatalogo_OkTest()
        {
            var datosDuplicadosConCatalogoInexistente = new DatosDuplicados(999_999, 1, 2);

            Assert.ThrowsException<ExcepcionCatalogo>(() =>
                _gestorControlClusters.ConfirmarCluster(datosDuplicadosConCatalogoInexistente));
        }

        [TestMethod]
        public void ConfirmarCluster_ItemInexistenteEnCatalogo_LanzaExcepcionItem_OkTest()
        {
            var itemExistenteEnCatalogo = new Item { Titulo = "A", Descripcion = "d" };
            _catalogo.AgregarItem(itemExistenteEnCatalogo);

            var datosDuplicadosConItemInexistente = new DatosDuplicados(
                _catalogo.Id,
                itemExistenteEnCatalogo.Id,
                9_999);

            Assert.ThrowsException<ExcepcionItem>(() =>
                _gestorControlClusters.ConfirmarCluster(datosDuplicadosConItemInexistente));
        }

        [TestMethod]
        public void
            BorrarItemDelCluster_ItemPerteneceACluster_EliminaItemDelClusterYSiQuedaBajoMinimoEliminaCluster_OkTest()
        {
            var itemQueDebePermanecerEnCluster = new Item { Titulo = "A", Descripcion = "d" };
            var itemQueDebeSerEliminadoDelCluster = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemQueDebePermanecerEnCluster);
            _catalogo.AgregarItem(itemQueDebeSerEliminadoDelCluster);

            _catalogo.CrearCluster(new HashSet<Item>
                { itemQueDebePermanecerEnCluster, itemQueDebeSerEliminadoDelCluster });

            var datosParaRemoverItemDelCluster = new DatosRemoverItemCluster(
                itemQueDebeSerEliminadoDelCluster.Id, _catalogo.Id);

            _gestorControlClusters.BorrarItemDelCluster(datosParaRemoverItemDelCluster);

            Assert.AreEqual(0, _catalogo.Clusters.Count);
            Assert.IsNotNull(
                _catalogo.ObtenerItemPorId(itemQueDebeSerEliminadoDelCluster.Id)); // el ítem sigue en el catálogo
        }

        [TestMethod]
        public void BorrarItemDelCluster_ItemRemovidoEraCanonico_NulificaCanonicoSiElClusterSigueExistiendo_OkTest()
        {
            var itemQueDebeSerCanonicoInicial = new Item
                { Titulo = "Candidato", Descripcion = "descripcion mucho mas larga", Marca = "AC" };
            var itemNoCanonico = new Item { Titulo = "Otro", Descripcion = "descripcion" };
            var itemTerceroParaMantenerCluster = new Item { Titulo = "Tercero", Descripcion = "desc" };

            _catalogo.AgregarItem(itemQueDebeSerCanonicoInicial);
            _catalogo.AgregarItem(itemNoCanonico);
            _catalogo.AgregarItem(itemTerceroParaMantenerCluster);

            _catalogo.CrearCluster(new HashSet<Item>
                { itemQueDebeSerCanonicoInicial, itemNoCanonico, itemTerceroParaMantenerCluster });
            var clusterExistenteConCanonico = _catalogo.Clusters.First();
            clusterExistenteConCanonico.FusionarCanonico();

            var datosParaRemoverCanonico = new DatosRemoverItemCluster(
                itemQueDebeSerCanonicoInicial.Id, _catalogo.Id);

            _gestorControlClusters.BorrarItemDelCluster(datosParaRemoverCanonico);

            Assert.AreEqual(1, _catalogo.Clusters.Count);
            Assert.IsNull(clusterExistenteConCanonico.Canonico);
        }

        [TestMethod]
        public void BorrarItemDelCluster_ItemSinCluster_NoRealizaCambios_OkTest()
        {
            var itemQueNoEstaEnNingunCluster = new Item { Titulo = "A", Descripcion = "d" };
            _catalogo.AgregarItem(itemQueNoEstaEnNingunCluster);

            var datosParaRemoverItemSinCluster = new DatosRemoverItemCluster(
                itemQueNoEstaEnNingunCluster.Id, _catalogo.Id);

            _gestorControlClusters.BorrarItemDelCluster(datosParaRemoverItemSinCluster);

            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void FusionarItemsEnElCluster_SinCanonicoPrevio_AsignaCanonicoSegunReglas_OkTest()
        {
            var itemConDescripcionMuyLarga = new Item { Titulo = "A", Descripcion = "descripcion muy larga" };
            var itemConDescripcionCorta = new Item { Titulo = "B", Descripcion = "corta" };
            _catalogo.AgregarItem(itemConDescripcionMuyLarga);
            _catalogo.AgregarItem(itemConDescripcionCorta);

            _catalogo.CrearCluster(new HashSet<Item> { itemConDescripcionMuyLarga, itemConDescripcionCorta });
            var clusterSinCanonicoPrevio = _catalogo.Clusters.First();

            var datosParaFusionarItems = new DatosFusionarItems(
                _catalogo.Id,
                clusterSinCanonicoPrevio.Id);

            _gestorControlClusters.FusionarItemsEnElCluster(datosParaFusionarItems);

            Assert.IsNotNull(clusterSinCanonicoPrevio.Canonico);
            Assert.AreSame(itemConDescripcionMuyLarga, clusterSinCanonicoPrevio.Canonico);
        }

        [TestMethod]
        public void FusionarItemsEnElCluster_ClusterInexistente_NoRealizaCambios_OkTest()
        {
            var cantidadInicialDeClusters = _catalogo.Clusters.Count;

            var datosParaFusionarItemsEnClusterInexistente = new DatosFusionarItems(
                _catalogo.Id,
                idCluster: 987_654_321);

            _gestorControlClusters.FusionarItemsEnElCluster(datosParaFusionarItemsEnClusterInexistente);

            Assert.AreEqual(cantidadInicialDeClusters, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ItemEstaEnCluster_ItemPerteneceAAlguno_ReturnsTrue_OkTest()
        {
            var itemQueDebeEstarEnUnCluster = new Item { Titulo = "A", Descripcion = "d" };
            _catalogo.AgregarItem(itemQueDebeEstarEnUnCluster);
            _catalogo.CrearCluster(new HashSet<Item> { itemQueDebeEstarEnUnCluster });

            var resultadoDePertenencia = _gestorControlClusters.ItemEstaEnCluster(
                _catalogo.Id,
                itemQueDebeEstarEnUnCluster.Id);

            Assert.IsTrue(resultadoDePertenencia);
        }

        [TestMethod]
        public void ItemEstaEnCluster_ItemNoPerteneceANinguno_ReturnsFalse_OkTest()
        {
            var itemQueNoPerteneceAClusters = new Item { Titulo = "B", Descripcion = "d" };
            _catalogo.AgregarItem(itemQueNoPerteneceAClusters);

            var resultadoDePertenencia = _gestorControlClusters.ItemEstaEnCluster(
                _catalogo.Id,
                itemQueNoPerteneceAClusters.Id);

            Assert.IsFalse(resultadoDePertenencia);
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_SinClusters_RetornaColeccionVacia_OkTest()
        {

            var clustersDto = _gestorControlClusters.ObtenerClustersDtoCatalogo(_catalogo.Id);

            Assert.IsNotNull(clustersDto);
            Assert.AreEqual(0, clustersDto.Count);
            Assert.AreEqual(0, _catalogo.Clusters.Count);
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_ConClustersExistentes_RetornaCantidadEIdsCorrectos_OkTest()
        {
            var itemA = new Item { Titulo = "A", Descripcion = "desc A" };
            var itemB = new Item { Titulo = "B", Descripcion = "desc B" };
            var itemC = new Item { Titulo = "C", Descripcion = "desc C" };
            var itemD = new Item { Titulo = "D", Descripcion = "desc D" };

            _catalogo.AgregarItem(itemA);
            _catalogo.AgregarItem(itemB);
            _catalogo.AgregarItem(itemC);
            _catalogo.AgregarItem(itemD);

            var duplicadoAyB = new DatosDuplicados(_catalogo.Id, itemA.Id, itemB.Id);
            var duplicadoCyD = new DatosDuplicados(_catalogo.Id, itemC.Id, itemD.Id);

            var respuestaAyB = _gestorControlClusters.ConfirmarCluster(duplicadoAyB);
            var respuestaCyD = _gestorControlClusters.ConfirmarCluster(duplicadoCyD);

            Assert.IsTrue(respuestaAyB, "La confirmación del cluster A~B debería devolver true.");
            Assert.IsTrue(respuestaCyD, "La confirmación del cluster C~D debería devolver true.");
            Assert.AreEqual(2, _catalogo.Clusters.Count, "Deben existir 2 clusters en el catálogo.");

            var idsClustersDominio = _catalogo.Clusters.Select(c => c.Id).OrderBy(x => x).ToArray();
            
            var clustersDto = _gestorControlClusters.ObtenerClustersDtoCatalogo(_catalogo.Id);
            
            Assert.AreEqual(2, clustersDto.Count, "La cantidad de DTOs debe coincidir con los clusters del dominio.");

            var idsClustersDto = clustersDto.Select(d => d.Id).OrderBy(x => x).ToArray();
            CollectionAssert.AreEqual(idsClustersDominio, idsClustersDto,
                "Los IDs de clusters deben mapear 1:1 en los DTOs.");
        }

        [TestMethod]
        public void ObtenerClustersDtoCatalogo_CatalogoInexistente_LanzaExcepcionCatalogo_OkTest()
        {
            var ex = Assert.ThrowsException<ExcepcionCatalogo>(
                () => _gestorControlClusters.ObtenerClustersDtoCatalogo(9999));

            Assert.AreEqual("Catálogo no encontrado (Id=9999).", ex.Message);
        }
        
        [TestMethod]
        public void DatosPublicosCluster_Id_MapeaIgual_OkTest()
        {
            var itemA = new Item { Titulo = "A", Descripcion = "desc A" };
            var itemB = new Item { Titulo = "B", Descripcion = "desc B" };
            _catalogo.AgregarItem(itemA);
            _catalogo.AgregarItem(itemB);
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, itemA.Id, itemB.Id));

            var cluster = _catalogo.Clusters.First();
            var dto = DatosPublicosCluster.FromEntity(cluster);

            Assert.AreEqual(cluster.Id, dto.Id);
        }

        [TestMethod]
        public void DatosPublicosCluster_Pertenecientes_CantidadCorrecta_OkTest()
        {
            var itemA = new Item { Titulo = "A", Descripcion = "desc A" };
            var itemB = new Item { Titulo = "B", Descripcion = "desc B" };
            _catalogo.AgregarItem(itemA);
            _catalogo.AgregarItem(itemB);
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, itemA.Id, itemB.Id));

            var dto = DatosPublicosCluster.FromEntity(_catalogo.Clusters.First());

            Assert.AreEqual(2, dto.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void DatosPublicosCluster_Canonico_NoEsNulo_OkTest()
        {
            var itemA = new Item { Titulo = "A", Descripcion = "desc A bien larga" };
            var itemB = new Item { Titulo = "B", Descripcion = "desc B" };
            _catalogo.AgregarItem(itemA);
            _catalogo.AgregarItem(itemB);
            _gestorControlClusters.ConfirmarCluster(new DatosDuplicados(_catalogo.Id, itemA.Id, itemB.Id));
            
            var cluster = _catalogo.Clusters.First();
            
            _gestorControlClusters.FusionarItemsEnElCluster(new DatosFusionarItems(_catalogo.Id, cluster.Id));

            var dto = DatosPublicosCluster.FromEntity(_catalogo.Clusters.First());

            Assert.IsNotNull(dto.Canonico);
        }
    }
}
