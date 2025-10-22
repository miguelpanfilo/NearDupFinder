using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Dominio.Clases
{
    [TestClass]
    public class GestorItemsTests
    {
        private static GestorCatalogos gestorCatalogos;
        private static GestorControlDuplicados gestorControlDuplicados;
        private static GestorAuditoria gestorAuditoria;
        private static HashSet<int> idsItemsGlobal;
        private static GestorItems gestorItems;
        private static Catalogo catalogo;

        [ClassInitialize]
        public static void Inicializar(TestContext context)
        {
            var almacenamiento = new NearDupFinder_Almacenamiento.AlmacenamientoDeDatos();
            gestorCatalogos = new GestorCatalogos(almacenamiento);
            gestorControlDuplicados = new GestorControlDuplicados();
            gestorAuditoria = new GestorAuditoria();
            idsItemsGlobal = new HashSet<int>();

            gestorItems = new GestorItems(gestorCatalogos, gestorControlDuplicados, gestorAuditoria, idsItemsGlobal);

            gestorCatalogos.CrearCatalogo(new DatosCatalogoCrear("Catalogo Test"));
            catalogo = gestorCatalogos.ObtenerCatalogoPorTitulo("Catalogo Test");
        }

        [TestMethod]
        public void CrearItem_AgregaItemAlCatalogo()
        {
            var nuevoItem = new DatosCrearItem
            {
                Titulo = "Item 1",
                Descripcion = "Descripción 1",
                IdCatalogo = catalogo.Id
            };

            gestorItems.CrearItem(nuevoItem);

            var items = gestorCatalogos.ObtenerItemsDelCatalogo(catalogo.Id);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("Item 1", items.First().Titulo);
            Assert.AreEqual("Descripción 1", items.First().Descripcion);
        }

        [TestMethod]
        [ExpectedException(typeof(ExcepcionItem))]
        public void CrearItem_TituloVacio_LanzaExcepcion()
        {
            var nuevoItem = new DatosCrearItem
            {
                Titulo = "",
                Descripcion = "Descripción 1",
                IdCatalogo = catalogo.Id
            };

            gestorItems.CrearItem(nuevoItem);
        }

        [TestMethod]
        public void ActualizarItemEnCatalogo_CambiaTituloYDescripcion()
        {
            // Crear item
            var datosCrear = new DatosCrearItem
            {
                Titulo = "Original",
                Descripcion = "Desc original",
                IdCatalogo = catalogo.Id
            };
            gestorItems.CrearItem(datosCrear);
            var item = catalogo.Items.First();

            // Actualizar
            var datosActualizar = new DatosActualizarItem
            {
                IdCatalogo = catalogo.Id,
                IdItem = item.Id,
                Titulo = "Nuevo Título",
                Descripcion = "Nueva Desc"
            };
            gestorItems.ActualizarItemEnCatalogo(datosActualizar);

            Assert.AreEqual("Nuevo Título", item.Titulo);
            Assert.AreEqual("Nueva Desc", item.Descripcion);
        }

        [TestMethod]
        public void IdExisteEnListaDeIdGlobal_ItemCreado_RetornaTrue()
        {
            var datosCrear = new DatosCrearItem
            {
                Titulo = "Item 2",
                Descripcion = "Desc 2",
                IdCatalogo = catalogo.Id
            };

            gestorItems.CrearItem(datosCrear);
            var item = catalogo.Items.First(i => i.Titulo == "Item 2");

            Assert.IsTrue(gestorItems.IdExisteEnListaDeIdGlobal(item.Id));
        }
    }
}
