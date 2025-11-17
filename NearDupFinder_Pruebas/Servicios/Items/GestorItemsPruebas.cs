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
using NearDupFinder_LogicaDeNegocio.Servicios.Notificaciones;
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
        private GestorNotificaciones _gestorNotificaciones = null!;
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
            IRepositorioNotificaciones repoNotificaciones = new RepositorioNotificaciones(_context);
            _repoDuplicados = new RepositorioDuplicados(_context);

            var sesionUsuario = new SesionUsuarioActual();
            sesionUsuario.Asignar("tester@correo.com");

            _gestorAuditoria = new GestorAuditoria(repoAuditorias, sesionUsuario);
            _gestorCatalogos = new GestorCatalogos(repoCatalogos,repoClusters, repoItems);
            _gestorNotificaciones = new GestorNotificaciones(repoNotificaciones);
            _gestorControlClusters = new GestorControlClusters(
                _gestorCatalogos,
                _gestorAuditoria,
                _gestorNotificaciones,
                sesionUsuario,
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
        
        [TestMethod]
        public void CrearItem_ConPrecioEImagen_LosSeteaCorrectamente()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            string base64 = Convert.ToBase64String(bytes);

            var dto = new DatosCrearItem(
                IdCatalogo: _catalogo.Id,
                Titulo: "Item con precio e imagen",
                Descripcion: "Descripción",
                Categoria: "Categoria 1",
                Marca: "Marca 1",
                Modelo: "Modelo 1",
                Stock: 10,
                Precio: 999,
                ImagenBase64: base64
            );

            var item = _controladorItems.CrearItem(dto);

            Assert.AreEqual("Item con precio e imagen", item.Titulo);
            Assert.AreEqual("Descripción", item.Descripcion);
            Assert.AreEqual(10, item.Stock);
            Assert.AreEqual(999, item.Precio);
            Assert.AreEqual(base64, item.ImagenBase64);
        }
        
        [TestMethod]
        public void ActualizarItemEnCatalogo_ModificaPrecioStockEImagen()
        {
            var item = new Item("Original", "Descripción original", 1)
            {
                Categoria = "Cat 1",
                Marca = "Marca 1",
                Modelo = "Modelo 1"
            };

            item.EditarPrecio(100);

            var imagenInicial = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            item.EditarImagen(imagenInicial);

            _catalogo.AgregarItem(item);
            _idsItemsGlobal.Add(item.Id);
            _context.Items.Add(item);
            _context.SaveChanges();

            var nuevaImagen = Convert.ToBase64String(new byte[] { 9, 8, 7 });

            var dto = new DatosActualizarItem(
                IdCatalogo: _catalogo.Id,
                IdItem: item.Id,
                Titulo: "Nuevo Título",
                Descripcion: "Nueva Descripción",
                Categoria: "Cat 2",
                Marca: "Marca 2",
                Modelo: "Modelo 2",
                Stock: 50,
                Precio: 777,
                ImagenBase64: nuevaImagen
            );

            _controladorItems.ActualizarItemEnCatalogo(dto);

            Assert.AreEqual("Nuevo Título", item.Titulo);
            Assert.AreEqual("Nueva Descripción", item.Descripcion);
            Assert.AreEqual("Cat 2", item.Categoria);
            Assert.AreEqual("Marca 2", item.Marca);
            Assert.AreEqual("Modelo 2", item.Modelo);

            Assert.AreEqual(50, item.Stock);
            Assert.AreEqual(777, item.Precio);
            Assert.AreEqual(nuevaImagen, item.ImagenBase64);
        }

        [TestMethod]
        public void AsegurarIdUnico_IdExistente_IncrementaHastaEncontrarLibre()
        {
            Item.ResetearContadorId(1);

            var itemExistente = Item.Crear("Item existente", "Desc");
            _context.Items.Add(itemExistente);
            _context.SaveChanges();

            int idExistente = itemExistente.Id; 

            var nuevoItem = new Item("Nuevo", "Desc", 0); 
            nuevoItem.AjustarId(idExistente);             

            _gestorItems.AsegurarIdUnico(nuevoItem);

            Assert.AreEqual(idExistente + 1, nuevoItem.Id);
        }

        [TestMethod]
        public void AsegurarIdUnico_VariosIdsConsecutivos_BuscaHastaElLibre()
        {
            Item.ResetearContadorId(1);

            var item1 = Item.Crear("A", "D"); 
            var item2 = Item.Crear("B", "D"); 
            var item3 = Item.Crear("C", "D");

            _context.Items.AddRange(item1, item2, item3);
            _context.SaveChanges();

            var nuevo = new Item("Nuevo", "Desc", 0); 
            nuevo.AjustarId(1); 

            _gestorItems.AsegurarIdUnico(nuevo);

            Assert.AreEqual(4, nuevo.Id);
        }
        [TestMethod]
        public void AsegurarIdUnico_IdNoExistente_NoModificaId()
        {
            Item.ResetearContadorId(1);
    
            var item = Item.Crear("Nuevo item", "Desc");

            int idOriginal = item.Id;  

            _gestorItems.AsegurarIdUnico(item);

            Assert.AreEqual(idOriginal, item.Id,
                "Si el ID no existe en repositorio, debe mantenerse el mismo.");
        }
 
    }
}
