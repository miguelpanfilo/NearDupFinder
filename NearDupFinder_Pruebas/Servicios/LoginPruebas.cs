using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios
{
    [TestClass]
    public class LoginPruebas
    {
        private Login _login = null!;
        private AlmacenamientoDeDatos _almacenamiento = null!;

        [TestInitialize]
        public void Setup()
        {
            _almacenamiento = new AlmacenamientoDeDatos();
            _login = new Login(_almacenamiento);
        }

        [TestMethod]
        public void AutenticarUsuario_CredencialesValidas_RetornaUsuario()
        {
           
            string email = "admin@gmail.com"; 
            string clave = "123QWEasdzxc@";    

            var datos = new DatosAutenticacion(email, clave);
            
            var usuario = _login.AutenticarUsuario(datos);
            
            Assert.IsNotNull(usuario);
            Assert.AreEqual(email, usuario.Email.ToString());
        }

        [TestMethod]
        public void AutenticarUsuario_ClaveIncorrecta_RetornaNull()
        {
            string email = "admin@gmail.com";
            string claveIncorrecta = "ClaveMala123@";
            var datos = new DatosAutenticacion(email, claveIncorrecta);

           
            var usuario = _login.AutenticarUsuario(datos);

            Assert.IsNull(usuario);
        }
        [TestMethod]
        public void AutenticarUsuario_UsuarioInexistente_RetornaNull()
        {
            
            string email = "noexiste@neardupfinder.com";
            string clave = "123QWEasdzxc@";
            var datos = new DatosAutenticacion(email, clave);
            
            var usuario = _login.AutenticarUsuario(datos);
            
            Assert.IsNull(usuario, "Debe devolver null si el usuario no existe en el almacenamiento.");
        }

        [TestMethod]
        public void AutenticarUsuario_EmailInvalido_LanzaExcepcion()
        {
            string email = "admin@@neardupfinder";
            string clave = "123QWEasdzxc@";
            var datos = new DatosAutenticacion(email, clave);

            var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
                _login.AutenticarUsuario(datos)
            );

            StringAssert.Contains(ex.Message, "El email no tiene un formato válido.");
        }
    }
}