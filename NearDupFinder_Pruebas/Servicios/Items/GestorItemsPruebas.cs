using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_Pruebas.Utilidades;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Pruebas.Servicios.Items
{
    [TestClass]
    public class GestorItemPruebas
    {
        private ControladorItems _controladorItems = null!;
        private GestorItems _gestorItems = null!;
        private GestorCatalogos _gestorCatalogos = null!;
        private GestorAuditoria _gestorAuditoria = null!;
        private GestorControlClusters _gestorControlClusters = null!;
        private Catalogo _catalogo = null!;
        private HashSet<int> _idsItemsGlobal = null!;
        private List<ParDuplicado> _duplicadosGlobales = null!;
        private SqlContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            var procesador = new ProcesadorTexto();

            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory(nameof(GestorItemPruebas));
            _context = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);

            IRepositorioItems repoItems = new RepositorioItems(_context);
            IRepositorioCatalogos repoCatalogos = new RepositorioCatalogos(_context);
            IRepositorioClusters repoClusters = new RepositorioClusters(_context);

            _gestorAuditoria = new GestorAuditoria();
            _gestorCatalogos = new GestorCatalogos(repoCatalogos);
            _gestorControlClusters = new GestorControlClusters(_gestorCatalogos, _gestorAuditoria, repoCatalogos,repoClusters,repoItems);
            _idsItemsGlobal = new HashSet<int>();
            _duplicadosGlobales = new List<ParDuplicado>();

            _gestorItems = new GestorItems(_idsItemsGlobal, repoItems);

            var gestorDuplicados = new GestorDuplicados(procesador);

            var controladorDuplicados = new ControladorDuplicados(
                _gestorAuditoria,
                gestorDuplicados,
                _gestorCatalogos,
                _gestorControlClusters,
                _duplicadosGlobales
            );

            _catalogo = new Catalogo("Catálogo Auditoría Test");
            repoCatalogos.Agregar(_catalogo);
            repoCatalogos.GuardarCambios();

            _controladorItems = new ControladorItems(
                _gestorItems,
                _gestorCatalogos,
                controladorDuplicados,
                _gestorControlClusters,
                _gestorAuditoria,
                _idsItemsGlobal
            );
        }

        [TestMethod]
        public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
        {
            var item = new Item("Original", "Descripción original")
            {
                Categoria = "Cat 1",
                Marca = "Marca 1",
                Modelo = "Modelo 1"
            };

            _catalogo.AgregarItem(item);
            _idsItemsGlobal.Add(item.Id);
            _context.Items.Add(item);
            _context.SaveChanges();

            var dto = new DatosActualizarItem(
                IdCatalogo: _catalogo.Id,
                IdItem: item.Id,
                Titulo: "Nuevo Título",
                Descripcion: "Nueva Descripción",
                Categoria: "Cat 1",
                Marca: "Marca 1",
                Modelo: "Modelo 1"
            );

            _controladorItems.ActualizarItemEnCatalogo(dto);

            Assert.AreEqual("Nuevo Título", item.Titulo);
            Assert.AreEqual("Nueva Descripción", item.Descripcion);
        }

        [TestMethod]
        public void CantidadDeItemsGlobal_SinItems_RetornaCero()
        {
            int numeroDeItems = _idsItemsGlobal.Count;

            Assert.AreEqual(0, numeroDeItems);
        }

        [TestMethod]
        public void CantidadDeItemsGlobal_ConItems_RetornaDistintoDeCero()
        {
            var dto = new DatosCrearItem(
                IdCatalogo: _catalogo.Id,
                Titulo: "Item 1",
                Descripcion: "Descripción 1"
            );
            _controladorItems.CrearItem(dto);
            int numeroDeItems = _idsItemsGlobal.Count;
            
            Assert.AreEqual(1, numeroDeItems);
        }

        [TestMethod]
        public void IdExisteEnListaDeIdGlobal_ConItemNoExistente_RetornaFalso()
        {
            var nuevoItem = new Item("Item 1", "Descripción 1");

            bool existeItem = _gestorItems.IdExisteEnListaDeIdGlobal(nuevoItem.Id);

            Assert.IsFalse(existeItem);
        }

        [TestMethod]
        public void IdExisteEnListaDeIdGlobal_ConItemExistente_RetornaVerdadero()
        {
            var dto = new DatosCrearItem(
                IdCatalogo: _catalogo.Id,
                Titulo: "Item 1",
                Descripcion: "Descripción 1"
            );
            var item = _controladorItems.CrearItem(dto);
            
            bool existeItem = _gestorItems.IdExisteEnListaDeIdGlobal(item.Id);

            Assert.IsTrue(existeItem);
        }
    }
}
