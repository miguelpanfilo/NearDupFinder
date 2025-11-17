using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaGestorAuditoria;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFinder_Pruebas.Utilidades;


namespace NearDupFinder_Pruebas.Dominio.Log
{
    [TestClass]
    public class ControladorAuditoriaPruebas
    {
        private SqlContext _contexto = null!;
        private RepositorioAuditorias _repositorio = null!;
        private GestorAuditoria _gestorAuditoria = null!;
        private ControladorAuditoria _controladorAuditoria = null!;
        private SesionUsuarioActual _sesion = null!;

        [TestInitialize]
        public void Setup()
        {
            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BaseDeDatosDePrueba_ControladorAuditoria");
            _contexto = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_contexto);

            _repositorio = new RepositorioAuditorias(_contexto);

            _sesion = new SesionUsuarioActual();
            _sesion.Asignar("tester@correo.com");

            _gestorAuditoria = new GestorAuditoria(_repositorio, _sesion);

            _controladorAuditoria = new ControladorAuditoria(_gestorAuditoria);
        }

        [TestMethod]
        public void ObtenerTodos_DeberiaDevolverSoloDTOs()
        {
            _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Test");

            var resultados = _controladorAuditoria.ObtenerTodos();

            Assert.AreEqual(1, resultados.Count);
            Assert.IsInstanceOfType(resultados[0], typeof(AuditoriaDto));
        }

        [TestMethod]
        public void ObtenerTodos_DeberiaEstarOrdenadoPorFecha()
        {
            _gestorAuditoria.RegistrarLogManual(DateTime.Now.AddMinutes(-10), "A", EntradaDeLog.AccionLog.AltaUsuario, "Primero");
            _gestorAuditoria.RegistrarLogManual(DateTime.Now, "B", EntradaDeLog.AccionLog.AltaUsuario, "Segundo");

            var resultados = _controladorAuditoria.ObtenerTodos();

            Assert.IsTrue(resultados[0].Timestamp < resultados[1].Timestamp,
                "Los DTOs deben estar ordenados por Timestamp ascendente.");
        }

        [TestMethod]
        public void ObtenerTodos_MapeaCorrectamenteCampos()
        {
            var fecha = new DateTime(2025, 01, 01, 10, 30, 00);
            _gestorAuditoria.RegistrarLogManual(fecha, "user@correo.com", EntradaDeLog.AccionLog.AltaUsuario, "Detalle test");

            var dto = _controladorAuditoria.ObtenerTodos().First();

            Assert.AreEqual("user@correo.com", dto.Usuario);
            Assert.AreEqual("AltaUsuario", dto.Accion);
            Assert.AreEqual("Detalle test", dto.Detalles);
            Assert.AreEqual(fecha, dto.Timestamp);
        }

        [TestMethod]
        public void ObtenerTodos_SinLogs_DeberiaDevolverListaVacia()
        {
            var resultados = _controladorAuditoria.ObtenerTodos();

            Assert.AreEqual(0, resultados.Count);
        }
    }
}
