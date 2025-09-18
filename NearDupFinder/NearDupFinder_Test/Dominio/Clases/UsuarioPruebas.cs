using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class UsuarioPruebas
{
    [TestMethod]
    public void Crear_ConDatosValidos_DevuelveInstancia()
    {
        Email email = Email.Crear("manuelperezmartirene@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
       var usuario = Usuario.Crear("Manuel","Perez",email,fecha);
        
        Assert.AreEqual("Manuel", usuario.Nombre);
        Assert.AreEqual("Perez", usuario.Apellido);
        Assert.AreEqual("manuelperezmartirene@gmail.com", usuario.Email.ToString());
        Assert.AreEqual("12-27-1997", usuario.FechaNacimiento.ToString());
    }
    
    [TestMethod]
    public void Crear_EmailNulo_LanzaArgumentNullException()
    {
        var fecha = Fecha.Crear(1997, 12, 27);

        Assert.ThrowsException<ArgumentNullException>(() => Usuario.Crear("Manuel", "Perez", null!, fecha));
    }
    
    [TestMethod]
    public void Crear_FechaNula_LanzaArgumentNullException()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");

        Assert.ThrowsException<ArgumentNullException>(() => Usuario.Crear("Manuel", "Perez", email, null!));
    }
    
    [TestMethod]
    public void Crear_NombreVacio_LanzaExcepcion()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);

        Assert.ThrowsException<ArgumentException>(() => Usuario.Crear("", "Perez", email, fecha));
        Assert.ThrowsException<ArgumentException>(() => Usuario.Crear("   ", "Perez", email, fecha));
    }
    
    [TestMethod]
    public void Crear_ApellidoVacio_LanzaExcepcion()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);

        Assert.ThrowsException<ArgumentException>(() => Usuario.Crear("Manuel", "", email, fecha));
        Assert.ThrowsException<ArgumentException>(() => Usuario.Crear("Manuel", "   ", email, fecha));
    }
    
    [TestMethod]
    public void Agregar_RolValido()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.AgregarRol(Rol.Administrador);
        Assert.IsTrue(usuario.TieneRol(Rol.Administrador));
    }
    
    [TestMethod]
    public void Agregar_RolRepetido()
    {
        var email = Email.Crear("manuel@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.AgregarRol(Rol.Revisor);
        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
        Assert.AreEqual(1, usuario.ObtenerRoles().Count);
    }
    
    [TestMethod]
    public void Remover_RolValido()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.AgregarRol(Rol.Revisor);
        usuario.RemoverRol(Rol.Revisor);

        Assert.IsFalse(usuario.TieneRol(Rol.Revisor));
    }
    
    [TestMethod]
    public void Remover_RolInvalido()
    {
        var email = Email.Crear("manuel@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.RemoverRol(Rol.Administrador);

        Assert.IsFalse(usuario.TieneRol(Rol.Administrador));
        Assert.AreEqual(0, usuario.ObtenerRoles().Count);
    }

    [TestMethod]
    public void PuedeCoexistir_AdministradorYRevisor_DevuelveVerdadero()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(usuario.TieneRol(Rol.Administrador));
        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
        Assert.AreEqual(2, usuario.ObtenerRoles().Count);
    }

    [TestMethod]
    public void ListadoRoles_SinDuplicados()
    {
        var email = Email.Crear("manuelperezmartirene@gmail.com");
        var fecha = Fecha.Crear(1997, 12, 27);
        var usuario = Usuario.Crear("Manuel", "Perez", email, fecha);

        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Administrador);
        usuario.AgregarRol(Rol.Revisor);

        CollectionAssert.AreEquivalent(new[] { Rol.Administrador, Rol.Revisor }, usuario.ObtenerRoles().ToArray());   
    }
    
    [TestMethod]
    public void Igual_Valido()
    {
        var usuario1 = Usuario.Crear("Manuel", "Perez", Email.Crear("manuel@ejemplo.com"), Fecha.Crear(1990, 2, 2));
        var usuario2 = Usuario.Crear("Manuel", "Perez", Email.Crear("MANUEL@EJEMPLO.COM"), Fecha.Crear(2000, 3, 3));

        Assert.IsTrue(usuario1.Igual(usuario2));
    }

    [TestMethod]
    public void Igual_Invalido()
    {
        var usuario1 = Usuario.Crear("Manuel", "Perez", Email.Crear("manuel@ejemplo.com"), Fecha.Crear(1990, 2, 2));
        var usuario2 = Usuario.Crear("Juan", "Perez", Email.Crear("juan@ejemplo.com"), Fecha.Crear(1991, 3, 3));

        Assert.IsFalse(usuario1.Igual(usuario2));
    }

    [TestMethod]
    public void Igual_Nulo_DevuelveFalse()
    {
        var usuario = Usuario.Crear("Manuel", "Perez", Email.Crear("manuel@ejemplo.com"), Fecha.Crear(1990, 2, 2));

        Assert.IsFalse(usuario.Igual(null!));
    }
}