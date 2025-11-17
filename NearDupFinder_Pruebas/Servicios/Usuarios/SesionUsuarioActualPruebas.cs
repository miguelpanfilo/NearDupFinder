using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Usuarios
{
    [TestClass]
    public class SesionUsuarioActualPruebas
    {
        private SesionUsuarioActual _sesion = null!;

        [TestInitialize]
        public void Setup()
        {
            _sesion = new SesionUsuarioActual();
        }

        [TestMethod]
        public void EstadoInicial_DeberiaSerNoHayUsuarioLogueado()
        {
            Assert.AreEqual("No hay usuario logueado", _sesion.EmailActual);
        }

        [TestMethod]
        public void Asignar_ConEmailValido_DeberiaActualizarEmailActual()
        {
            _sesion.Asignar("admin@gmail.com");

            Assert.AreEqual("admin@gmail.com", _sesion.EmailActual);
        }

        [TestMethod]
        public void Desasignar_DeberiaRestablecerElEstadoPorDefecto()
        {
            _sesion.Asignar("admin@gmail.com");

            _sesion.Desasignar();

            Assert.AreEqual("No hay usuario logueado", _sesion.EmailActual);
        }
    }
}
