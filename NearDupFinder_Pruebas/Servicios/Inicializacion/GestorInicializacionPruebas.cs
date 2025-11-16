using System.Reflection;
using Moq;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Inicializacion;

namespace NearDupFinder_Pruebas.Servicios.Inicializacion
{
    [TestClass]
    public class GestorInicializacionPruebas
    {
        private Mock<IRepositorioUsuarios> _repoUsuariosMock = null!;
        private Mock<IRepositorioSincronizacionIds> _repoSyncMock = null!;
        private GestorInicializacion _sut = null!;

        [TestInitialize]
        public void SetUp()
        {
            _repoUsuariosMock = new Mock<IRepositorioUsuarios>();
            _repoSyncMock = new Mock<IRepositorioSincronizacionIds>();

            _sut = new GestorInicializacion(
                _repoUsuariosMock.Object,
                _repoSyncMock.Object
            );

            _repoSyncMock.Setup(r => r.ObtenerMaximoIdItems()).Returns(0);
            _repoSyncMock.Setup(r => r.ObtenerMaximoIdCatalogos()).Returns(0);
            _repoSyncMock.Setup(r => r.ObtenerMaximoIdCluster()).Returns(0);
        }

        [TestMethod]
        public void AsegurarInicializacion_SinAdmin_LlamaAgregarAdminUnaVez()
        {
            _repoUsuariosMock
                .Setup(r => r.ObtenerUsuarioPorEmail("admin@gmail.com"))
                .Returns((Usuario?)null);

            _sut.AsegurarInicializacion();

            _repoUsuariosMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void AsegurarInicializacion_ConAdmin_NoLlamaAgregar()
        {
            var admin = Usuario.Crear(
                "admin",
                "existente",
                Email.Crear("admin@gmail.com"),
                Fecha.Crear(1990, 1, 1)
            );

            _repoUsuariosMock
                .Setup(r => r.ObtenerUsuarioPorEmail("admin@gmail.com"))
                .Returns(admin);

            _sut.AsegurarInicializacion();

            _repoUsuariosMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Never);
        }

        [TestMethod]
        public void AsegurarInicializacion_DosVeces_ConsultaIdsSoloUnaVez()
        {
            _repoUsuariosMock
                .Setup(r => r.ObtenerUsuarioPorEmail("admin@gmail.com"))
                .Returns((Usuario?)null);

            _sut.AsegurarInicializacion();
            _sut.AsegurarInicializacion();

            _repoUsuariosMock.Verify(r => r.ObtenerIdMaximo(), Times.Once);
        }
        
        [TestMethod]
        public void AsegurarInicializacion_IdInicialMayorAValorPorDefecto_ActualizaNextId()
        {
            var field = typeof(Usuario)
                .GetField("_nextId", BindingFlags.Static | BindingFlags.NonPublic);
            field!.SetValue(null, 1);

            _repoUsuariosMock.Setup(r => r.ObtenerIdMaximo()).Returns(5);

            _repoUsuariosMock
                .Setup(r => r.ObtenerUsuarioPorEmail("admin@gmail.com"))
                .Returns((Usuario?)null);

            Usuario? usuarioCreado = null;

            _repoUsuariosMock
                .Setup(r => r.Agregar(It.IsAny<Usuario>()))
                .Callback<Usuario>(u => usuarioCreado = u);

            _sut.AsegurarInicializacion();

            Assert.AreEqual(6, usuarioCreado!.Id);
        }
    }
}
