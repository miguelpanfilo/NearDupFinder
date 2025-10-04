using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Test.Dominio;

[TestClass]
public class SistemaPruebas
{
    private Sistema _sistema;
    private Catalogo _catalogo;
    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _catalogo = new Catalogo("Catalogo Test"); 
        _sistema.AgregarCatalogo(_catalogo);
    }
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
    public void ModificarClave_CamposValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarClave_UsuarioInexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "noexiste@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "admin.gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailVacio_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? email = null;
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string claveNueva = "invalida";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveVacia_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string claveNueva = "";

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveNula_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string? claveNueva = null;

        bool modificado = sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);
        Usuario? admin = sistema.AutenticarUsuario(email, claveNueva);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(admin);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_NoPermiteAutenticarConClaveVieja()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";
        string claveVieja = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = sistema.ModificarClave(email, claveNueva);
        Usuario? conVieja = sistema.AutenticarUsuario(email, claveVieja);

        Assert.IsTrue(modificado);
        Assert.IsNull(conVieja);
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
    // Fin Pruebas Usuario

    // Inicio Pruebas Items
    
    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        catalogo.AgregarItem(item);
        var dto = new ItemEditDataTransfer
        {
            Id = item.Id,
            Titulo = "Nuevo Título",
            Descripcion = "Nueva Descripción",
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };

        sistema.ActualizarItemEnCatalogo(catalogo, dto);

        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }
    
    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
    {
        
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        catalogo.AgregarItem(item);

        var dto = new ItemEditDataTransfer
        {
            Id = item.Id,
            Titulo = "Original",
            Descripcion = "Descripcion original",
            Categoria = "Cat 2",
            Marca = "Marca 2",
            Modelo = "Modelo 2"
        };

        sistema.ActualizarItemEnCatalogo(catalogo, dto);

        
        Assert.AreEqual("Cat 2", item.Categoria);
        Assert.AreEqual("Marca 2", item.Marca);
        Assert.AreEqual("Modelo 2", item.Modelo);
    }
    [TestMethod]
    public void ActualizarItemEnCatalogo_ItemNoExiste_Excepcion()
    {
        
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");

        var dto = new ItemEditDataTransfer
        {
            Id = 999, // Id inexistente
            Titulo = "Título",
            Descripcion = "Descripcion",
            Categoria = "Cat",
            Marca = "Marca",
            Modelo = "Modelo"
        };

        var ex = Assert.ThrowsException<ItemException>(
            () => sistema.ActualizarItemEnCatalogo(catalogo, dto)
        );

        Assert.AreEqual("No se encontró el item a actualizar.", ex.Message);
    }
    [TestMethod]
    public void AltaItem_AgregaItemAlCatalogo()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("Item 1", "Descripción 1");

        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
        var items = catalogo.Items;

        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Item 1", items.First().Titulo);
        Assert.AreEqual("Descripción 1", items.First().Descripcion);
    }
    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_SinCatalogo_Excepcion()
    {
        
        var sistema = new Sistema();
        var nuevoItem = new Item("Item 1", "Desc");

        sistema.AltaItemConAltaDuplicados("Inexistente", nuevoItem);

    }

    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_LanzaExcepcionSiTituloOVacio()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("", "Descripción 1"); 
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }


    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_DescripcionVacia_Excepcion()
    {
        var sistema = new Sistema();
        var catalogo = new Catalogo("Catálogo Test");
        sistema.AgregarCatalogo(catalogo);

        var nuevoItem = new Item("Titulo", ""); 
        sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }
    
  
    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnListaGlobal()
    {
        
        var catalogo = new Catalogo("Catálogo Test");
        _sistema.AgregarCatalogo(catalogo); // Necesitás este método en tu sistema

        var item1 = new Item("Titulo 1", "Descripcion 1");
        var item2 = new Item("Titulo 1", "Descripcion 1");

        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item2);

        Assert.AreEqual(1, _sistema.DuplicadosGlobales.Count);
    }
    [TestMethod]
    public void ActualizarDuplicados_MarcaEstadoDuplicadoEnItems()
    {
        var item1 = new Item("Titulo", "Descripcion");
        var item2 = new Item("Titulo", "Descripcion");
        _catalogo.AgregarItem(item1);
        _catalogo.AgregarItem(item2);

        _sistema.ActualizarDuplicadosPara(_catalogo, item1);

        Assert.IsTrue(item1.EstadoDuplicado);
        Assert.IsTrue(item2.EstadoDuplicado);
    }
    



    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarDuplicadosPara_ExcepcionSiCatalogoEsNull()
    {
        var item = new Item("Titulo", "Descripcion");
        _sistema.ActualizarDuplicadosPara(null, item);
    }
    
    
    
}