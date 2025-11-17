using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Pruebas.Utilidades;

namespace NearDupFinder_Pruebas.Repositorios
{
    [TestClass]
    public class RepositorioSincronizacionPruebas
    {
        private SqlContext _contexto = null!;
        private RepositorioSincronizacionIds _repositorio = null!;

        [TestInitialize]
        public void Setup()
        {
            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_SyncIdsPruebas");
            _contexto = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_contexto);

            _repositorio = new RepositorioSincronizacionIds(_contexto);
        }

        [TestMethod]
        public void ObtenerMaximoIdItems_SinItems_RetornaCero()
        {
            int maxId = _repositorio.ObtenerMaximoIdItems();

            Assert.AreEqual(0, maxId, "Sin items, el máximo debe ser 0.");
        }

        [TestMethod]
        public void ObtenerMaximoIdItems_ConItems_RetornaMayorId()
        {
            var item1 = Item.Crear("A", "Desc");
            var item2 = Item.Crear("B", "Desc");
            var item3 = Item.Crear("C", "Desc");

            _contexto.Items.AddRange(item1, item2, item3);
            _contexto.SaveChanges();

            int maxEsperado = new[] { item1.Id, item2.Id, item3.Id }.Max();

            int maxId = _repositorio.ObtenerMaximoIdItems();

            Assert.AreEqual(maxEsperado, maxId, "Debe retornar el ID más alto.");
        }
        
        [TestMethod]
        public void ObtenerMaximoIdCatalogos_SinCatalogos_RetornaCero()
        {
            int maxId = _repositorio.ObtenerMaximoIdCatalogos();

            Assert.AreEqual(0, maxId);
        }

        [TestMethod]
        public void ObtenerMaximoIdCatalogos_ConCatalogos_RetornaMayorId()
        {
            var c1 = new Catalogo("C1");
            var c2 = new Catalogo("C2");
            var c3 = new Catalogo("C3");

            _contexto.Catalogos.AddRange(c1, c2, c3);
            _contexto.SaveChanges();

            int maxEsperado = new[] { c1.Id, c2.Id, c3.Id }.Max();

            int maxId = _repositorio.ObtenerMaximoIdCatalogos();

            Assert.AreEqual(maxEsperado, maxId);
        }
        [TestMethod]
        public void ObtenerMaximoIdCluster_SinClusters_RetornaCero()
        {
            int maxId = _repositorio.ObtenerMaximoIdCluster();

            Assert.AreEqual(0, maxId);
        }

    }
}
