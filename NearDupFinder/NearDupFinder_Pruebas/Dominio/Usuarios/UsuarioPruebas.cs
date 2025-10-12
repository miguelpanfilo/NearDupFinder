using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Pruebas.Dominio.Usuarios;

[TestClass]
public class UsuarioPruebas
{ 
    private static Email CrearEmail(string email) => Email.Crear(email);
    private static Fecha CrearFecha(int a, int m, int d) => Fecha.Crear(a, m, d);
    private static Usuario CrearUsuario(
        string nombre = "Manuel",
        string apellido = "Perez",
        string mail = "manuel@ejemplo.com",
        int a = 1997, int m = 12, int d = 27) =>
        Usuario.Crear(nombre, apellido, CrearEmail(mail), CrearFecha(a, m, d));
    private const string ClaveValida = "123QWEasdzxc@";
    private static readonly string ClaveInvalida = string.Empty;


    [TestMethod]
    public void CrearUsuario_NombreVacio_LanzaUsuarioException()
    {
        var email = CrearEmail("manuel@ejemplo.com");
        var fecha = CrearFecha(1997, 12, 27);
        string nombre = "";
        string apellido = "Perez";

        Assert.ThrowsException<UsuarioException>(() => Usuario.Crear(nombre, apellido, email, fecha));
    }

    [TestMethod]
    public void CrearUsuario_NombreEspacioEnBlanco_LanzaUsuarioException()
    {
        var email = CrearEmail("manuel@ejemplo.com");
        var fecha = CrearFecha(1997, 12, 27);
        string nombre = " ";
        string apellido = "Perez";

        Assert.ThrowsException<UsuarioException>(() => Usuario.Crear(nombre, apellido, email, fecha));
    }

    [TestMethod]
    public void CrearUsuario_ApellidoVacio_LanzaUsuarioException()
    {
        var email = CrearEmail("manuel@ejemplo.com");
        var fecha = CrearFecha(1997, 12, 27);
        string nombre = "Manuel";
        string apellido = "";

        Assert.ThrowsException<UsuarioException>(() => Usuario.Crear(nombre, apellido, email, fecha));
    }

    [TestMethod]
    public void CrearUsuario_ApellidoEspacioEnBlanco_LanzaUsuarioException()
    {
        var email = CrearEmail("manuel@ejemplo.com");
        var fecha = CrearFecha(1997, 12, 27);
        string nombre = "Manuel";
        string apellido = " ";

        Assert.ThrowsException<UsuarioException>(() => Usuario.Crear(nombre, apellido, email, fecha));
    }
    
    [TestMethod]
    public void CrearUsuario_TrimEnNombreYApellido_RetornaUsuarioCorrecto()
    {
        var usuario = Usuario.Crear("  Manuel  ", "  Perez  ", CrearEmail("m@e.com"), CrearFecha(2000, 1, 1));

        Assert.AreEqual("Manuel", usuario.Nombre);
        Assert.AreEqual("Perez", usuario.Apellido);
    }

    [TestMethod]
    public void CrearUsuario_UsuarioConDatosValidos_DevuelveInstanciaValida()
    {
        var email = CrearEmail("manuelperezmartirene@gmail.com");
        var fecha = CrearFecha(1997, 12, 27);

        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        Assert.AreEqual("Manuel", usuario.Nombre);
        Assert.AreEqual("Perez", usuario.Apellido);
        Assert.AreEqual("manuelperezmartirene@gmail.com", usuario.Email.ToString());
        Assert.AreEqual("12-27-1997", usuario.FechaNacimiento.ToString());
    }

    [TestMethod]
    public void CrearUsuario_IdsSecuenciales_IncrementaEnUno()
    {
        var email1 = CrearEmail("manuelperezmartirene@gmail.com");
        var fecha1 = CrearFecha(1997, 12, 27);
        var usuario1 = Usuario.Crear("Manuel", "Perez", email1, fecha1);

        var email2 = CrearEmail("juanperezmartirene@gmail.com");
        var fecha2 = CrearFecha(2000, 12, 18);
        var usuario2 = Usuario.Crear("Juan", "Perez", email2, fecha2);

        int diferencia = usuario2.Id - usuario1.Id;

        Assert.AreEqual(1, diferencia);
    }
    
    [TestMethod]
    public void UsuarioNuevo_SinRoles_IniciaVacio()
    {
        var usuario = CrearUsuario();
        
        Assert.AreEqual(0, usuario.ObtenerRoles().Count);
    }

    [TestMethod]
    public void AgregarRol_AgregaRolAdministrador_QuedaAsignado()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Administrador);

        Assert.IsTrue(usuario.TieneRol(Rol.Administrador));
    }
    
    [TestMethod]
    public void AgregarRol_AgregaRolRevisor_QuedaAsignado()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
    }

    [TestMethod]
    public void AgregarRol_RolRepetido_NoDuplica()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);
        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
        Assert.AreEqual(1, usuario.ObtenerRoles().Count);
    }
    
    [TestMethod]
    public void AgregarRol_PuedenCoexistir_AdminYRevisor()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(usuario.TieneRol(Rol.Administrador));
        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
        Assert.AreEqual(2, usuario.ObtenerRoles().Count);
    }
    
    [TestMethod]
    public void RemoverRol_Existente_SeElimina()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);
        usuario.RemoverRol(Rol.Revisor);

        Assert.IsFalse(usuario.TieneRol(Rol.Revisor));
    }

    [TestMethod]
    public void RemoverRol_Inexistente_NoModifica()
    {
        var usuario = CrearUsuario();

        usuario.RemoverRol(Rol.Administrador);

        Assert.IsFalse(usuario.TieneRol(Rol.Administrador));
        Assert.AreEqual(0, usuario.ObtenerRoles().Count);
    }

    [TestMethod]
    public void ObtenerRoles_AgregandoRolesIguales_RetornaSinDuplicados()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Revisor);
        var esperado = new[] { Rol.Administrador, Rol.Revisor };

        CollectionAssert.AreEquivalent(esperado, usuario.ObtenerRoles().ToArray());
    }
    
    [TestMethod]
    public void Igual_EmailsEquivalentes_RetornaVerdadero()
    {
        var usuario1 = Usuario.Crear("Manuel", "Perez", CrearEmail("manuel@ejemplo.com"), CrearFecha(1990, 2, 2));
        var usuario2 = Usuario.Crear("Manuel", "Perez", CrearEmail("MANUEL@EJEMPLO.COM"), CrearFecha(2000, 3, 3));

        Assert.IsTrue(usuario1.Igual(usuario2));
    }

    [TestMethod]
    public void Igual_EmailsDistintos_RetornaFalso()
    {
        var usuario1 = Usuario.Crear("Manuel", "Perez", CrearEmail("manuel@ejemplo.com"), CrearFecha(1990, 2, 2));
        var usuario2 = Usuario.Crear("Juan", "Perez", CrearEmail("juan@ejemplo.com"), CrearFecha(1991, 3, 3));

        Assert.IsFalse(usuario1.Igual(usuario2));
    }
    
    [TestMethod]
    public void VerificarClave_ClaveInValida_RetornaFalso()
    {
        var usuario = CrearUsuario();

        bool exito = usuario.VerificarClave(ClaveInvalida);

        Assert.IsFalse(exito);
    }
    
    [TestMethod]
    public void CambiarClave_ClaveValida_RetornaVerdadero()
    {
        var usuario = CrearUsuario();
        var clave = Clave.Crear(ClaveValida);

        bool exito = usuario.CambiarClave(clave) && usuario.VerificarClave(ClaveValida);

        Assert.IsTrue(exito);
    }
}