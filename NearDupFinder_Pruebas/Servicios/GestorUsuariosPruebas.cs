using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio;

namespace NearDupFinder_Pruebas.Servicios;

[TestClass]
public class GestorUsuariosPruebas
{
    private static Email CrearEmail(string email) => Email.Crear(email);
    private static Fecha CrearFecha(int a, int m, int d) => Fecha.Crear(a, m, d);
    private static Usuario CrearUsuario(
        string nombre = "Manuel",
        string apellido = "Perez",
        string mail = "manuel@ejemplo.com",
        int a = 1997, int m = 12, int d = 27) =>
        Usuario.Crear(nombre, apellido, CrearEmail(mail), CrearFecha(a, m, d));
    
    [TestMethod]
    public void CrearUsuario_NombreVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ApellidoVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_FechaInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuel@gmail.com";
        int anio = 1997;
        int mes = 15;
        int dia = 49;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveInvalida";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_ClaveVacia_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_PuedeNoTenerRoles_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol>();

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_UsuarioYaExistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Administrador];
        string nombre2 = "Juan";
        string apellido2 = "Perez";
        
        sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        bool usuarioCreado = sistema.AltaUsuario(nombre2, apellido2, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Administrador];

        bool usuarioCreado = sistema.AltaUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }
    
    [TestMethod]
    public void ModificarUsuario_NombreVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string nombreVacio = "";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombreVacio, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ApellidoVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string apellidoVacio = "";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellidoVacio, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInvalido = "manuel.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, emailInvalido, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_FechaInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        int diaInvalido = 100;
        int mesInvalido = 13;
        int anioInvalido = 0;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anioInvalido, mesInvalido, diaInvalido, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveInvalida = "Invalida";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, claveInvalida, roles);

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_ClaveVacia_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = string.Empty;
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaNombreYApellido_RetornaVerdaderoYActualizaCampos()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string nombreNuevo = "NuevoNombre";
        string apellidoNuevo = "NuevoApellido";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = string.Empty;
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombreNuevo, apellidoNuevo, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(modificado);
        Assert.AreEqual(nombreNuevo, usuario.Nombre);
        Assert.AreEqual(apellidoNuevo, usuario.Apellido);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        Usuario? usuarioAutenticado = sistema.ValidarUsuario(email,clave);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(usuarioAutenticado);
    }

    [TestMethod]
    public void ModificarUsuario_RemplazaRoles_SoloQuedaListaNueva()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = string.Empty;
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        var rolesUsuario = usuario.ObtenerRoles().ToList();
        
        CollectionAssert.AreEquivalent(roles, rolesUsuario);
        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_RolesVacio_EliminaTodosLosRoles()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = string.Empty;
        List<Rol> roles = [];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        var rolesUsuario = usuario.ObtenerRoles().ToList();
        
        CollectionAssert.AreEquivalent(roles, rolesUsuario);
        Assert.IsTrue(modificado);
        Assert.AreEqual(0, rolesUsuario.Count);
    }
    
    [TestMethod]
    public void ModificarUsuario_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        Usuario usuario = CrearUsuario(nombre,apellido,email,anio,mes,dia);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = string.Empty;
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        
        Assert.IsTrue(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_UsuarioInexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manu";
        string apellido = "Pérez";
        string email = "noexiste@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        
        bool usuarioRemovido = sistema.EliminarUsuario(email);
        
        Assert.IsTrue(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_Inexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "asdasdasda@gmail.com";
        
        bool usuarioRemovido = sistema.EliminarUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "manuel.com";
        
        bool usuarioRemovido = sistema.EliminarUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void ModificarClave_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(email, claveActual, claveNueva);

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarClave_UsuarioInexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInexistente = "noexiste@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(emailInexistente, claveActual, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInexistente = "noexiste@.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(emailInexistente, claveActual, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "invalida";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(email, claveActual, claveNueva);
        
        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarClave_ClaveActualInCorrecta_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveActualInvalida = "NuevaClaveValida123!";
        string claveNueva = "Encr1pt4d0@";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(email, claveActualInvalida, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "Encr1pt4d0@";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(email, claveActual, claveNueva);
        Usuario? admin = sistema.ValidarUsuario(email, claveNueva);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(admin);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_NoPermiteAutenticarConClaveVieja()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "Encr1pt4d0@";

        usuario.CambiarClave(Clave.Crear(claveActual));
        bool modificado = sistema.ModificarClave(email, claveActual, claveNueva);
        Usuario? admin = sistema.ValidarUsuario(email, claveActual);

        Assert.IsTrue(modificado);
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_Correcto_RetornaUsuario()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "123QWEasdzxc@";

        usuario.CambiarClave(Clave.Crear(clave));
        Usuario? admin = sistema.ValidarUsuario(email, clave);
        
        Assert.IsNotNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "123QWEasdzxc@";
        string claveIncorrecta = "Incorrecta@!";

        usuario.CambiarClave(Clave.Crear(clave));
        Usuario? admin = sistema.ValidarUsuario(email, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailIncorrecto_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailIncorrecto = "incorrecto@gmail.com";
        Usuario usuario = CrearUsuario(nombre,apellido,email);
        sistema.AgregarUsuarioALaLista(usuario);
        string clave = "123QWEasdzxc@";

        usuario.CambiarClave(Clave.Crear(clave));
        Usuario? admin = sistema.ValidarUsuario(emailIncorrecto, clave);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void UsuarioTieneRol_TieneRolRevisor_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(sistema.UsuarioTieneRol(usuario, Rol.Revisor));
    }
    
    [TestMethod]
    public void UsuarioTieneRol_NoTieneRolRevisor_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        var usuario = CrearUsuario();
        
        Assert.IsFalse(sistema.UsuarioTieneRol(usuario, Rol.Revisor));
    }
    
    [TestMethod]
    public void ObtenerRolesDeUsuario_TieneRolRevisor_RetornaListaConRolRevisor()
    {
        Sistema sistema = new Sistema();
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);
        var listaDeRolesDelUsuario = sistema.ObtenerRolesDeUsuario(usuario);
        
        Assert.AreEqual(1, listaDeRolesDelUsuario.Count());
        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
    }
}