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
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

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
        private ControladorDuplicados _controladorDuplicados = null!;
        private GestorDuplicados _gestorDuplicados = null!;
        private Catalogo _catalogo = null!;
        private HashSet<int> _idsItemsGlobal = null!;
        private SqlContext _context = null!;
        private IRepositorioDuplicados _repoDuplicados = null!;

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
            IRepositorioAuditorias repoAuditorias = new RepositorioAuditorias(_context);
            _repoDuplicados = new RepositorioDuplicados(_context);

            var sesionUsuario = new SesionUsuarioActual();
            sesionUsuario.Asignar("tester@correo.com");

            _gestorAuditoria = new GestorAuditoria(repoAuditorias, sesionUsuario);
            _gestorCatalogos = new GestorCatalogos(repoCatalogos,repoClusters, repoItems);

            _gestorControlClusters = new GestorControlClusters(
                _gestorCatalogos,
                _gestorAuditoria,
                repoCatalogos,
                repoClusters,
                repoItems
            );

            _idsItemsGlobal = new HashSet<int>();
            _gestorItems = new GestorItems(repoItems);

            _gestorDuplicados = new GestorDuplicados(procesador);

            _controladorDuplicados = new ControladorDuplicados(
                _gestorAuditoria,
                _gestorDuplicados,
                _gestorCatalogos,
                _gestorControlClusters,
                _repoDuplicados
            );

            _catalogo = new Catalogo("Catálogo Auditoría Test");
            repoCatalogos.Agregar(_catalogo);
            repoCatalogos.GuardarCambios();

            _controladorItems = new ControladorItems(
                _gestorItems,
                _gestorCatalogos,
                _controladorDuplicados,
                _gestorControlClusters,
                _gestorAuditoria);
        }

        [TestMethod]
        public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
        {
            var item = new Item("Original", "Descripción original",0)
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
        public void IdExisteEnListaDeIdGlobal_ConItemNoExistente_RetornaFalso()
        {
            var nuevoItem = new Item("Item 1", "Descripción 1",0);

            bool existeItem = _gestorItems.ExisteItemConEseId(nuevoItem.Id);

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

            bool existeItem = _gestorItems.ExisteItemConEseId(item.Id);

            Assert.IsTrue(existeItem);
        }
    }
}
