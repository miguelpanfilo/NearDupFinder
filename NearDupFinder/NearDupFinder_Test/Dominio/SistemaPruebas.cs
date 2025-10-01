using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio;

[TestClass]
public class SistemaPruebas
{
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
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_NombreNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = null;
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

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
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ApellidoNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = null;
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

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
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_AnioInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = -12;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_MesInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 15;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_DiaInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 15;
        int dia = 55;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveInvalida";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_ClaveVacia_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_ClaveNula_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = null;
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_SinRoles_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> {};

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_RolesNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol>? roles = null;

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_UsuarioYaExistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> {Rol.Administrador};
        string nombre2 = "Juan";
        string? apellido2 = "Perez";
        
        sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        bool usuarioCreado = sistema.CrearUsuario(nombre2, apellido2, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> {Rol.Administrador};

        bool usuarioCreado = sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDNoExsite_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        int idInexistente = Int32.MaxValue;

        Usuario? usuarioABuscar = sistema.BuscarUsuarioPorId(idInexistente);
        
        Assert.IsNull(usuarioABuscar);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorId_IDExsite_RetornaUsuarioValido()
    {
        Sistema sistema = new Sistema();
        Usuario? admin = sistema.ObtenerUsuarios().FirstOrDefault();
        int idValido = admin!.Id;
        
        Usuario? usuarioABuscar = sistema.BuscarUsuarioPorId(idValido);
        
        Assert.AreEqual(admin,usuarioABuscar);
    }

    [TestMethod]
    public void ModificarUsuario_NombreVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ApellidoVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin.gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_FechaInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 15;
        int dia = 40;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "invalida";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaNombreYApellido_RetornaVerdaderoYActualizaCampos()
    {
        Sistema sistema = new Sistema();
        Usuario? admin = sistema.ObtenerUsuarios().FirstOrDefault();
        string? email = admin!.Email.ToString();
        string? nombre = "NuevoNombre";
        string? apellido = "NuevoApellido";
        int anio = 1994;
        int mes = 7;
        int dia = 21;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Administrador };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        Usuario? actualizado = sistema.BuscarUsuarioPorId(admin.Id);

        Assert.IsTrue(modificado);
        Assert.AreEqual(nombre, actualizado!.Nombre);
        Assert.AreEqual(apellido, actualizado!.Apellido);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        Sistema sistema = new Sistema();
        string? email = "admin@gmail.com";
        string claveNueva = "ClaveMuySegura123@";
        bool modificado = sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, claveNueva, new List<Rol> { Rol.Administrador });
        Usuario? autenticado = sistema.AutenticarUsuario(email, claveNueva);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(autenticado);
    }

    [TestMethod]
    public void ModificarUsuario_RemplazaRoles_SoloQuedaListaNueva()
    {
        Sistema sistema = new Sistema();
        string? email = "admin@gmail.com";
        List<Rol> rolesNuevos = new List<Rol> { Rol.Revisor };
        bool modificado = sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, "ClaveValida123!", rolesNuevos);
        Usuario? usuario = sistema.AutenticarUsuario(email, "ClaveValida123!");

        var rolesUsuario = usuario!.ObtenerRoles().ToList();
        CollectionAssert.AreEquivalent(rolesNuevos, rolesUsuario);
        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_RolesVacio_EliminaTodosLosRoles()
    {
        Sistema sistema = new Sistema();
        string? email = "admin@gmail.com";
        List<Rol> rolesVacios = new List<Rol>();
        bool modificado = sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, "ClaveValida123!", rolesVacios);
        Usuario? usuario = sistema.AutenticarUsuario(email, "ClaveValida123!");

        var rolesUsuario = usuario!.ObtenerRoles().ToList();
        Assert.AreEqual(0, rolesUsuario.Count);
        Assert.IsTrue(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_RolesNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Perez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol>? roles = null;

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "NuevaClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_UsuarioInexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "noexiste@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void AutenticoUsuario_Correto_RetornaUsuario()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveAdmin);
        
        Assert.IsNotNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_Incorrecto_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveIncorrecta= "mal";
        
        Usuario? admin = sistema.AutenticarUsuario(emailIncorrecto, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailIncorrecto_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailIncorrecto, claveAdmin);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string claveIncorrecta= "mal";
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailNulo_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string? emailNulo = null;
        string claveAdmin = "123QWEasdzxc@";
        
        Usuario? admin = sistema.AutenticarUsuario(emailNulo, claveAdmin);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveNula_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string emailAdmin = "admin@gmail.com";
        string? claveNula= null;
        
        Usuario? admin = sistema.AutenticarUsuario(emailAdmin, claveNula);
        
        Assert.IsNull(admin);
    }

    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";

        // La clase sistema comienza con el Usuario admin por defecto por lo tanto no es necesario depender de agregar 
        // a un usuario para este test
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
    public void RemoverUsuario_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? email = null;
        
        bool usuarioRemovido = sistema.EliminarUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
}