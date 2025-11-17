using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFInder_LogicaDeNegocio.Servicios.ReservasStock;

namespace NearDupFinder_Pruebas.Servicios.Rerservas
{
    [TestClass]
    public class GestorReservasPruebas
    {
        [TestInitialize]
        public void ResetearIds()
        {
            Item.ResetearContadorId();
        }

        [TestMethod]
        public void Aplicar_ReparteDesdeLosMenores_YNoTocaLosMayores()
        {
            var i1 = ItemCon(id: 1, stock: 3);
            var i2 = ItemCon(id: 2, stock: 5);
            var i3 = ItemCon(id: 3, stock: 10);
            var cluster = ClusterCon(i1, i2, i3);

            int reservar = 8;

            bool ok = GestorReservas.Aplicar(cluster, reservar);

            Assert.IsTrue(ok);
            Assert.AreEqual(0, i1.Stock, "Item Id=1 debe agotarse primero.");
            Assert.AreEqual(0, i2.Stock, "Item Id=2 debe agotarse luego.");
            Assert.AreEqual(10, i3.Stock, "Item Id=3 no debe tocarse.");
            Assert.AreEqual(10, cluster.StockActual);
        }

        [TestMethod]
        public void Aplicar_DesempataPorId_CuandoStocksIguales()
        {
            var i2 = ItemCon(id: 2, stock: 4);
            var i1 = ItemCon(id: 1, stock: 4);
            var cluster = ClusterCon(i2, i1);

            int reservar = 5;

            bool ok = GestorReservas.Aplicar(cluster, reservar);

            Assert.IsTrue(ok);
            Assert.AreEqual(0, i1.Stock, "Debe consumirse primero el Id=1 por desempate.");
            Assert.AreEqual(3, i2.Stock, "Luego resta 1 del Id=2.");
        }

        [TestMethod]
        public void Aplicar_CantidadExacta_AgotaTodoYDevuelveTrue()
        {

            var a = ItemCon(10, 2);
            var b = ItemCon(20, 3);
            var c = ItemCon(30, 5);
            var cluster = ClusterCon(a, b, c);
            int total = cluster.StockActual;

            bool ok = GestorReservas.Aplicar(cluster, total);

            Assert.IsTrue(ok);
            CollectionAssert.AreEqual(new[] { 0, 0, 0 }, new[] { a.Stock, b.Stock, c.Stock });
            Assert.AreEqual(0, cluster.StockActual);
        }

        [TestMethod]
        public void Aplicar_NoImportaOrdenInicial_OrdenaInternamente()
        {
            var i5 = ItemCon(5, 7);
            var i1 = ItemCon(1, 1);
            var i4 = ItemCon(4, 3);
            var cluster = ClusterCon(i5, i1, i4);

            int reservar = 4;
            
            bool ok = GestorReservas.Aplicar(cluster, reservar);

            Assert.IsTrue(ok);
            Assert.AreEqual(0, i1.Stock);
            Assert.AreEqual(0, i4.Stock);
            Assert.AreEqual(7, i5.Stock, "El de 7 no debe tocarse.");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-10)]
        public void Aplicar_CantidadInvalida_LanzaInvalidOperationException(int cantidadInvalida)
        {
            var cluster = ClusterCon(ItemCon(1, 5));

            var ex = Assert.ThrowsException<InvalidOperationException>(
                () => GestorReservas.Aplicar(cluster, cantidadInvalida)
            );
            Assert.IsTrue(ex.Message.ToLower().Contains("cantidad inválida"));
        }

        [TestMethod]
        public void Aplicar_StockInsuficiente_LanzaInvalidOperationException()
        {
            var cluster = ClusterCon(ItemCon(1, 2), ItemCon(2, 1)); // total = 3

            var ex = Assert.ThrowsException<InvalidOperationException>(
                () => GestorReservas.Aplicar(cluster, cantidad: 4)
            );
            Assert.IsTrue(ex.Message.ToLower().Contains("stock insuficiente"));
        }
        
        [TestMethod]
        public void Aplicar_ClusterSinPrecioOUmbral_LanzaExcepcionCluster()
        {
            var item = ItemCon(1, 5);

            var cluster = new Cluster(
                id: 999,
                pertenecientesCluster: new HashSet<Item> { item }
            );

            cluster.PrecioCanonico = null;
            cluster.StockMinimoCanonico = null;

            var ex = Assert.ThrowsException<ExcepcionCluster>(
                () => GestorReservas.Aplicar(cluster, 3)
            );

            Assert.IsTrue(
                ex.Message.Contains("precio y/o umbral canónico", StringComparison.OrdinalIgnoreCase)
            );
        }
        [TestMethod]
        public void Aplicar_ClusterSinPrecio_LanzaExcepcionCluster()
        {
            var item = ItemCon(1, 5);

            var cluster = new Cluster(
                id: 999,
                pertenecientesCluster: new HashSet<Item> { item }
            );

            cluster.PrecioCanonico = null;
            cluster.StockMinimoCanonico = 10;

            var ex = Assert.ThrowsException<ExcepcionCluster>(
                () => GestorReservas.Aplicar(cluster, 3)
            );

            Assert.IsTrue(
                ex.Message.Contains("precio y/o umbral canónico", StringComparison.OrdinalIgnoreCase)
            );
        }
        [TestMethod]
        public void Aplicar_ClusterSinUmbral_LanzaExcepcionCluster()
        {
            var item = ItemCon(1, 5);

            var cluster = new Cluster(
                id: 999,
                pertenecientesCluster: new HashSet<Item> { item }
            );

            cluster.PrecioCanonico = 100;
            cluster.StockMinimoCanonico = null;

            var ex = Assert.ThrowsException<ExcepcionCluster>(
                () => GestorReservas.Aplicar(cluster, 3)
            );

            Assert.IsTrue(
                ex.Message.Contains("precio y/o umbral canónico", StringComparison.OrdinalIgnoreCase)
            );
        }

        private static Item ItemCon(int id, int stock)
        {
            var it = new Item();
            it.Stock = stock;
            it.AjustarId(id);
            return it;
        }

        private static Cluster ClusterCon(params Item[] items)
        {
            var cluster = new Cluster(
                id: 999,
                pertenecientesCluster: new HashSet<Item>(items)
            );

            cluster.PrecioCanonico = 100;
            cluster.StockMinimoCanonico = 1;

            return cluster;
        }
        
    }
}
