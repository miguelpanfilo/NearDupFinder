using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;


namespace NearDupFinder_Pruebas.Dominio.Clases
{
    [TestClass]
    public class ClusterPruebas
    {
        private static Item CrearItemConCamposDetallados(
            string titulo,
            string descripcion,
            string? marca = null,
            string? modelo = null,
            string? categoria = null)
        {
            return Item.Crear(titulo, descripcion, marca, modelo, categoria);
        }

        [TestMethod]
        public void Constructor_DeCluster_InicializaIdYMiembros_SinCanonico_OkTest()
        {
            var primerItemDePrueba = CrearItemConCamposDetallados("A", "DescripcionA");
            var segundoItemDePrueba = CrearItemConCamposDetallados("B", "DescripcionB");
            var conjuntoInicialDeMiembros = new HashSet<Item> { primerItemDePrueba, segundoItemDePrueba };

            var clusterBajoPrueba = new Cluster(id: 10, conjuntoInicialDeMiembros);

            Assert.AreEqual(10, clusterBajoPrueba.Id);
            CollectionAssert.AreEquivalent(new[] { primerItemDePrueba, segundoItemDePrueba }, clusterBajoPrueba.PertenecientesCluster.ToList());
            Assert.IsNull(clusterBajoPrueba.Canonico);
        }

        [TestMethod]
        public void Agregar_MiembroNoExistente_IncrementaCantidadYQuedaContenido_OkTest()
        {
            var itemNoExistenteEnCluster = CrearItemConCamposDetallados("A", "DescA");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item>());

            clusterBajoPrueba.Agregar(itemNoExistenteEnCluster);

            Assert.IsTrue(clusterBajoPrueba.Contiene(itemNoExistenteEnCluster));
            Assert.AreEqual(1, clusterBajoPrueba.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void Agregar_MismoObjetoItem_NoDuplicaPorEstructuraHashSet_OkTest()
        {
            var itemDuplicadoPorReferencia = CrearItemConCamposDetallados("A", "DescA");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemDuplicadoPorReferencia });

            clusterBajoPrueba.Agregar(itemDuplicadoPorReferencia);

            Assert.AreEqual(1, clusterBajoPrueba.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void Remover_MiembroExistente_LoEliminaSinAfectarResto_OkTest()
        {
            var itemAEliminarQueExiste = CrearItemConCamposDetallados("A", "DescA");
            var itemQuePermaneceEnCluster = CrearItemConCamposDetallados("B", "DescB");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemAEliminarQueExiste, itemQuePermaneceEnCluster });

            clusterBajoPrueba.Remover(itemAEliminarQueExiste);

            Assert.IsFalse(clusterBajoPrueba.Contiene(itemAEliminarQueExiste));
            Assert.IsTrue(clusterBajoPrueba.Contiene(itemQuePermaneceEnCluster));
            Assert.AreEqual(1, clusterBajoPrueba.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void Remover_MiembroInexistente_NoLanzaExcepcionYNiModificaCantidad_OkTest()
        {
            var itemQueNoPerteneceAlCluster = CrearItemConCamposDetallados("A", "DescA");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item>());

            clusterBajoPrueba.Remover(itemQueNoPerteneceAlCluster);

            Assert.AreEqual(0, clusterBajoPrueba.PertenecientesCluster.Count);
        }

        [TestMethod]
        public void Contiene_RetornaTrue_CuandoElItemSiPertenece_OkTest()
        {
            var itemQuePerteneceAlCluster = CrearItemConCamposDetallados("A", "DescA");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemQuePerteneceAlCluster });

            Assert.IsTrue(clusterBajoPrueba.Contiene(itemQuePerteneceAlCluster));
        }

        [TestMethod]
        public void FusionarCanonico_ConClusterVacio_RetornaFalseYCanonicoSigueNull_OkTest()
        {
            var clusterSinMiembros = new Cluster(1, new HashSet<Item>());

            var resultadoHuboCambioDeCanonico = clusterSinMiembros.FusionarCanonico();

            Assert.IsFalse(resultadoHuboCambioDeCanonico);
            Assert.IsNull(clusterSinMiembros.Canonico);
        }

        [TestMethod]
        public void FusionarCanonico_SeleccionaPorDescripcionLuegoTituloLuegoId_OkTest()
        {
            var itemConDescripcionMasLarga = CrearItemConCamposDetallados("TituloCorto", "Descripcion extremadamente larga");
            var itemConDescripcionMediaYTituloMasLargo = CrearItemConCamposDetallados("Titulo Considerablemente Largo", "Descripcion media");
            var itemConDescripcionMediaYTituloMasLargoIdMayor = CrearItemConCamposDetallados("Titulo Considerablemente Largo", "Descripcion media");

            var conjuntoMiembrosParaEvaluarCriterios = new HashSet<Item>
            {
                itemConDescripcionMediaYTituloMasLargo,
                itemConDescripcionMediaYTituloMasLargoIdMayor,
                itemConDescripcionMasLarga
            };

            var clusterBajoPrueba = new Cluster(1, conjuntoMiembrosParaEvaluarCriterios);

            var resultadoHuboCambioDeCanonico = clusterBajoPrueba.FusionarCanonico();

            Assert.IsTrue(resultadoHuboCambioDeCanonico);
            Assert.AreSame(itemConDescripcionMasLarga, clusterBajoPrueba.Canonico);
        }

        [TestMethod]
        public void FusionarCanonico_DobleInvocacionSinCambios_SegundaDevuelveFalse_OkTest()
        {
            var itemQueDebeSerCanonicoInicial = CrearItemConCamposDetallados("A", "Descripcion Bastante Larga");
            var itemConDescripcionMasCorta = CrearItemConCamposDetallados("B", "Corta");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemQueDebeSerCanonicoInicial, itemConDescripcionMasCorta });

            var resultadoPrimeraFusione = clusterBajoPrueba.FusionarCanonico();
            var resultadoSegundaFusione = clusterBajoPrueba.FusionarCanonico();

            Assert.IsTrue(resultadoPrimeraFusione);
            Assert.IsFalse(resultadoSegundaFusione);
            Assert.AreSame(itemQueDebeSerCanonicoInicial, clusterBajoPrueba.Canonico);
        }

        [TestMethod]
        public void FusionarCanonico_AlAgregarMiembroConMejoresCriterios_CambiaCanonicoYRetornaTrue_OkTest()
        {
            var itemCanonicoTemporal = CrearItemConCamposDetallados("A", "media");
            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemCanonicoTemporal });

            clusterBajoPrueba.FusionarCanonico();
            Assert.AreSame(itemCanonicoTemporal, clusterBajoPrueba.Canonico);

            var itemConDescripcionAunMasLargaParaDesplazarCanonico = CrearItemConCamposDetallados("B", "mucho mas larga");
            clusterBajoPrueba.Agregar(itemConDescripcionAunMasLargaParaDesplazarCanonico);

            var resultadoHuboCambioDeCanonico = clusterBajoPrueba.FusionarCanonico();

            Assert.IsTrue(resultadoHuboCambioDeCanonico);
            Assert.AreSame(itemConDescripcionAunMasLargaParaDesplazarCanonico, clusterBajoPrueba.Canonico);
        }

        [TestMethod]
        public void FusionarCampos_CuandoCanonicoYaTieneValores_NoSePisanConValoresDeOtrosMiembros_OkTest()
        {
            var itemCandidatoCanonicoConCamposDefinidos = CrearItemConCamposDetallados("A", "muy larga", marca: "AC", modelo: "M1", categoria: "X");
            var itemMiembroConCamposMasLargosPeroNoDebenPisar = CrearItemConCamposDetallados("B", "media", marca: "ACMECORP", modelo: "M999", categoria: "Categoria Enorme");

            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemCandidatoCanonicoConCamposDefinidos, itemMiembroConCamposMasLargosPeroNoDebenPisar });

            clusterBajoPrueba.FusionarCanonico();
            var itemCanonicoLuegoDeFusion = clusterBajoPrueba.Canonico!;

            Assert.AreEqual("AC", itemCanonicoLuegoDeFusion.Marca);
            Assert.AreEqual("M1", itemCanonicoLuegoDeFusion.Modelo);
            Assert.AreEqual("X", itemCanonicoLuegoDeFusion.Categoria);
        }

        [TestMethod]
        public void FusionarCampos_SinValoresEnCanonico_TomaMasLargoYDesempataLexicograficoAscendente_OkTest()
        {
            var itemCandidatoCanonicoSinCampos = CrearItemConCamposDetallados("A", "muy larga", marca: "", modelo: "", categoria: "");
            var itemConCamposMasLargos = CrearItemConCamposDetallados("B", "corta", marca: "ACMECO", modelo: "M123", categoria: "Categoria Larguísima");
            var itemEmpateLexicograficoMenor = CrearItemConCamposDetallados("C", "media", marca: "Beta", modelo: "M1", categoria: "X");
            var itemEmpateLexicograficoMayor = CrearItemConCamposDetallados("D", "media", marca: "Zeta", modelo: "M2", categoria: "Y");

            var clusterBajoPrueba = new Cluster(1, new HashSet<Item>
            {
                itemCandidatoCanonicoSinCampos,
                itemEmpateLexicograficoMayor,
                itemEmpateLexicograficoMenor,
                itemConCamposMasLargos
            });

            clusterBajoPrueba.FusionarCanonico();
            var itemCanonicoLuegoDeFusion = clusterBajoPrueba.Canonico!;

            Assert.AreEqual("ACMECO", itemCanonicoLuegoDeFusion.Marca);
            Assert.AreEqual("M123", itemCanonicoLuegoDeFusion.Modelo);
            Assert.AreEqual("Categoria Larguísima", itemCanonicoLuegoDeFusion.Categoria);

            var itemCanonicoVacíoParaEmpate = CrearItemConCamposDetallados("X", "super larga", marca: "", modelo: "", categoria: "");
            var itemMarcaEmpateLexicoMenor = CrearItemConCamposDetallados("Y", "m", marca: "Beta", modelo: "", categoria: "");
            var itemMarcaEmpateLexicoMayor = CrearItemConCamposDetallados("Z", "m", marca: "Zeta", modelo: "", categoria: "");
            var clusterParaProbarEmpateLexicografico = new Cluster(2, new HashSet<Item> { itemCanonicoVacíoParaEmpate, itemMarcaEmpateLexicoMenor, itemMarcaEmpateLexicoMayor });

            clusterParaProbarEmpateLexicografico.FusionarCanonico();
            var itemCanonicoEnEscenarioDeEmpate = clusterParaProbarEmpateLexicografico.Canonico!;

            Assert.AreEqual("Beta", itemCanonicoEnEscenarioDeEmpate.Marca);
        }

        [TestMethod]
        public void FusionarCampos_IgnoraCadenasVaciasONulasYAplicaTrim_OkTest()
        {
            var itemCandidatoCanonicoConCamposVaciosYEspacios = CrearItemConCamposDetallados("A", "larguísima", marca: "  ", modelo: "", categoria: null);
            var itemMiembroProveedorDeValoresValidosConEspacios = CrearItemConCamposDetallados("B", "media", marca: " AC ", modelo: "  M1", categoria: " X  ");

            var clusterBajoPrueba = new Cluster(1, new HashSet<Item> { itemCandidatoCanonicoConCamposVaciosYEspacios, itemMiembroProveedorDeValoresValidosConEspacios });

            clusterBajoPrueba.FusionarCanonico();
            var itemCanonicoLuegoDeFusion = clusterBajoPrueba.Canonico!;

            Assert.AreEqual("AC", itemCanonicoLuegoDeFusion.Marca);
            Assert.AreEqual("M1", itemCanonicoLuegoDeFusion.Modelo);
            Assert.AreEqual("X", itemCanonicoLuegoDeFusion.Categoria);
        }

        [TestMethod]
        public void FusionarCanonico_AlRemoverCanonicoActual_RecalculaConRestantesYFusionaCamposCorrectamente_OkTest()
        {
            var itemQueDebeSerCanonicoInicialPorDescripcion = CrearItemConCamposDetallados("A", "la mas larga", marca: "", modelo: "", categoria: "");
            var itemConCamposUtilesParaFusion = CrearItemConCamposDetallados("B", "media", marca: "ACME", modelo: "M1", categoria: "X");
            var itemConDescripcionCasiLargaQuePuedeVolverseCanonico = CrearItemConCamposDetallados("C", "casi larga", marca: "AC", modelo: "M2", categoria: "");

            var conjuntoInicialDeMiembros = new HashSet<Item>
            {
                itemQueDebeSerCanonicoInicialPorDescripcion,
                itemConCamposUtilesParaFusion,
                itemConDescripcionCasiLargaQuePuedeVolverseCanonico
            };

            var clusterBajoPrueba = new Cluster(1, conjuntoInicialDeMiembros);

            clusterBajoPrueba.FusionarCanonico();
            Assert.AreSame(itemQueDebeSerCanonicoInicialPorDescripcion, clusterBajoPrueba.Canonico);

            clusterBajoPrueba.Remover(itemQueDebeSerCanonicoInicialPorDescripcion);

            var resultadoHuboCambioDeCanonico = clusterBajoPrueba.FusionarCanonico();

            Assert.IsTrue(resultadoHuboCambioDeCanonico);
            Assert.AreSame(itemConDescripcionCasiLargaQuePuedeVolverseCanonico, clusterBajoPrueba.Canonico);

            var itemCanonicoLuegoDeRecalculo = clusterBajoPrueba.Canonico!;
            Assert.AreEqual("AC", itemCanonicoLuegoDeRecalculo.Marca);
            Assert.AreEqual("M2", itemCanonicoLuegoDeRecalculo.Modelo);
            Assert.AreEqual("X", itemCanonicoLuegoDeRecalculo.Categoria);
        }

        [TestMethod]
        public void StockActual_SumaStockDeTodosLosMiembros_OkTest()
        {
            Item.ResetearContadorId();

            var item1 = CrearItemConCamposDetallados("A", "DescA");
            var item2 = CrearItemConCamposDetallados("B", "DescB");
            var item3 = CrearItemConCamposDetallados("C", "DescC");

            item1.EditarStock(2);
            item2.EditarStock(3);
            item3.EditarStock(5);

            var miembros = new HashSet<Item> { item1, item2, item3 };
            var clusterBajoPrueba = new Cluster(1, miembros);

            var stockActual = clusterBajoPrueba.StockActual;

            Assert.AreEqual(2 + 3 + 5, stockActual);
        }

        [TestMethod]
        public void ConfigurarCanonico_ValoresValidos_SetearImagenStockYPrecio_OkTest()
        {
            var item1 = CrearItemConCamposDetallados("A", "DescA");
            var item2 = CrearItemConCamposDetallados("B", "DescB");
            var cluster = new Cluster(1, new HashSet<Item> { item1, item2 });

            string imagen = "aGVsbG8=";
            int stockMinimo = 10;
            int precio = 999;

            cluster.ConfigurarCanonico(imagen, stockMinimo, precio);

            Assert.AreEqual(imagen, cluster.ImagenCanonicaBase64);
            Assert.AreEqual(stockMinimo, cluster.StockMinimoCanonico);
            Assert.AreEqual(precio, cluster.PrecioCanonico);
        }

        [TestMethod]
        public void ConfigurarCanonico_StockMinimoNegativo_LanzaExcepcionItem_OkTest()
        {
            var item1 = CrearItemConCamposDetallados("A", "DescA");
            var cluster = new Cluster(1, new HashSet<Item> { item1 });

            Assert.ThrowsException<ExcepcionItem>(() =>
                cluster.ConfigurarCanonico("BASE64", -1, 100)
            );
        }

        [TestMethod]
        public void ConfigurarCanonico_PrecioNegativo_LanzaExcepcionItem_OkTest()
        {
            var item1 = CrearItemConCamposDetallados("A", "DescA");
            var cluster = new Cluster(1, new HashSet<Item> { item1 });

            Assert.ThrowsException<ExcepcionItem>(() =>
                cluster.ConfigurarCanonico("BASE64", 10, -5)
            );
        }
        
        [TestMethod]
        public void ConfigurarCanonico_ImagenBase64Invalida_LanzaExcepcionItemFormatoBase64_OkTest()
        {
            var cluster = new Cluster(1, new HashSet<Item>());

            var ex = Assert.ThrowsException<ExcepcionItem>(() =>
                cluster.ConfigurarCanonico("no-es-base64", stockMinimo: null, precio: null)
            );

            StringAssert.Contains(ex.Message, "formato Base64");
        }
        
        [TestMethod]
        public void ConfigurarCanonico_ImagenSuperaMaxBytes_LanzaExcepcionItemTamano_OkTest()
        {
            var cluster = new Cluster(1, new HashSet<Item>());

            var bytes = new byte[1024 * 1024 + 1];
            string base64Grande = Convert.ToBase64String(bytes);

            var ex = Assert.ThrowsException<ExcepcionItem>(() =>
                cluster.ConfigurarCanonico(base64Grande, stockMinimo: null, precio: null)
            );

            StringAssert.Contains(ex.Message, "1 MB");
        }
    }
}
