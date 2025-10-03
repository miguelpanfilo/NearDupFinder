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
        _catalogo = new Catalogo("Catálogo Test");
        _sistema.AgregarCatalogo(_catalogo);
    }

    // Inicio Pruebas Usuario
    [TestMethod]
    public void CrearUsuario_NombreVacio_RetornaFalso()
    {
        string nombre = "";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_NombreNulo_RetornaFalso()
    {
        string? nombre = null;
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ApellidoVacio_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ApellidoNulo_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = null;
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailVacio_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_EmailNulo_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_AnioInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = -12;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_MesInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 15;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_DiaInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string? email = null;
        int anio = 1997;
        int mes = 15;
        int dia = 55;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveInvalida";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveVacia_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveNula_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = null;
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_SinRoles_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_RolesNulo_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol>? roles = null;

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_UsuarioYaExistente_RetornaFalso()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Administrador };
        string nombre2 = "Juan";
        string? apellido2 = "Perez";

        _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        bool usuarioCreado = _sistema.CrearUsuario(nombre2, apellido2, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_CamposValidos_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string? apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string? clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Administrador };

        bool usuarioCreado = _sistema.CrearUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(usuarioCreado);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDNoExsite_RetornaNulo()
    {
        int idInexistente = Int32.MaxValue;

        Usuario? usuarioABuscar = _sistema.BuscarUsuarioPorId(idInexistente);

        Assert.IsNull(usuarioABuscar);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_IDExsite_RetornaUsuarioValido()
    {
        Usuario? admin = _sistema.ObtenerUsuarios().FirstOrDefault();
        int idValido = admin!.Id;

        Usuario? usuarioABuscar = _sistema.BuscarUsuarioPorId(idValido);

        Assert.AreEqual(admin, usuarioABuscar);
    }

    [TestMethod]
    public void ModificarUsuario_NombreVacio_RetornaFalso()
    {
        string? nombre = "";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ApellidoVacio_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_EmailInvalido_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin.gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_FechaInvalida_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 15;
        int dia = 40;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ClaveInvalida_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "invalida";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaNombreYApellido_RetornaVerdaderoYActualizaCampos()
    {
        Usuario? admin = _sistema.ObtenerUsuarios().FirstOrDefault();
        string? email = admin!.Email.ToString();
        string? nombre = "NuevoNombre";
        string? apellido = "NuevoApellido";
        int anio = 1994;
        int mes = 7;
        int dia = 21;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Administrador };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);
        Usuario? actualizado = _sistema.BuscarUsuarioPorId(admin.Id);

        Assert.IsTrue(modificado);
        Assert.AreEqual(nombre, actualizado!.Nombre);
        Assert.AreEqual(apellido, actualizado!.Apellido);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        string? email = "admin@gmail.com";
        string claveNueva = "ClaveMuySegura123@";
        bool modificado = _sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, claveNueva,
            new List<Rol> { Rol.Administrador });
        Usuario? autenticado = _sistema.AutenticarUsuario(email, claveNueva);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(autenticado);
    }

    [TestMethod]
    public void ModificarUsuario_RemplazaRoles_SoloQuedaListaNueva()
    {
        string? email = "admin@gmail.com";
        List<Rol> rolesNuevos = new List<Rol> { Rol.Revisor };
        bool modificado =
            _sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, "ClaveValida123!", rolesNuevos);
        Usuario? usuario = _sistema.AutenticarUsuario(email, "ClaveValida123!");

        var rolesUsuario = usuario!.ObtenerRoles().ToList();
        CollectionAssert.AreEquivalent(rolesNuevos, rolesUsuario);
        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_RolesVacio_EliminaTodosLosRoles()
    {
        string? email = "admin@gmail.com";
        List<Rol> rolesVacios = new List<Rol>();
        bool modificado =
            _sistema.ModificarUsuario("Admin", "Admin", email, 1990, 1, 1, "ClaveValida123!", rolesVacios);
        Usuario? usuario = _sistema.AutenticarUsuario(email, "ClaveValida123!");

        var rolesUsuario = usuario!.ObtenerRoles().ToList();
        Assert.AreEqual(0, rolesUsuario.Count);
        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_RolesNulo_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "Perez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol>? roles = null;

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_CamposValidos_RetornaVerdadero()
    {
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "admin@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "NuevaClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_UsuarioInexistente_RetornaFalso()
    {
        string? nombre = "Manu";
        string? apellido = "Pérez";
        string? email = "noexiste@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol> { Rol.Revisor };

        bool modificado = _sistema.ModificarUsuario(nombre, apellido, email, anio, mes, dia, clave, roles);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_CamposValidos_RetornaVerdadero()
    {
        string email = "admin@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarClave_UsuarioInexistente_RetornaFalso()
    {
        string email = "noexiste@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailInvalido_RetornaFalso()
    {
        string email = "admin.gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailVacio_RetornaFalso()
    {
        string email = "";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailNulo_RetornaFalso()
    {
        string? email = null;
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveInvalida_RetornaFalso()
    {
        string email = "admin@gmail.com";
        string claveNueva = "invalida";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveVacia_RetornaFalso()
    {
        string email = "admin@gmail.com";
        string claveNueva = "";

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveNula_RetornaFalso()
    {
        string email = "admin@gmail.com";
        string? claveNueva = null;

        bool modificado = _sistema.ModificarClave(email, claveNueva);

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        string email = "admin@gmail.com";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);
        Usuario? admin = _sistema.AutenticarUsuario(email, claveNueva);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(admin);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_NoPermiteAutenticarConClaveVieja()
    {
        string email = "admin@gmail.com";
        string claveVieja = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        bool modificado = _sistema.ModificarClave(email, claveNueva);
        Usuario? conVieja = _sistema.AutenticarUsuario(email, claveVieja);

        Assert.IsTrue(modificado);
        Assert.IsNull(conVieja);
    }

    [TestMethod]
    public void AutenticoUsuario_Correto_RetornaUsuario()
    {
        string emailAdmin = "admin@gmail.com";
        string claveAdmin = "123QWEasdzxc@";

        Usuario? admin = _sistema.AutenticarUsuario(emailAdmin, claveAdmin);

        Assert.IsNotNull(admin);
    }

    [TestMethod]
    public void AutenticoUsuario_Incorrecto_RetornaNulo()
    {
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveIncorrecta = "mal";

        Usuario? admin = _sistema.AutenticarUsuario(emailIncorrecto, claveIncorrecta);

        Assert.IsNull(admin);
    }

    [TestMethod]
    public void AutenticoUsuario_EmailIncorrecto_RetornaNulo()
    {
        string emailIncorrecto = "incorrecto@gmail.com";
        string claveAdmin = "123QWEasdzxc@";

        Usuario? admin = _sistema.AutenticarUsuario(emailIncorrecto, claveAdmin);

        Assert.IsNull(admin);
    }

    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        string emailAdmin = "admin@gmail.com";
        string claveIncorrecta = "mal";

        Usuario? admin = _sistema.AutenticarUsuario(emailAdmin, claveIncorrecta);

        Assert.IsNull(admin);
    }

    [TestMethod]
    public void AutenticoUsuario_EmailNulo_RetornaNulo()
    {
        string? emailNulo = null;
        string claveAdmin = "123QWEasdzxc@";

        Usuario? admin = _sistema.AutenticarUsuario(emailNulo, claveAdmin);

        Assert.IsNull(admin);
    }

    [TestMethod]
    public void AutenticoUsuario_ClaveNula_RetornaNulo()
    {
        string emailAdmin = "admin@gmail.com";
        string? claveNula = null;

        Usuario? admin = _sistema.AutenticarUsuario(emailAdmin, claveNula);

        Assert.IsNull(admin);
    }

    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        string email = "admin@gmail.com";

        // La clase sistema comienza con el Usuario admin por defecto por lo tanto no es necesario depender de agregar 
        // a un usuario para este test
        bool usuarioRemovido = _sistema.EliminarUsuario(email);

        Assert.IsTrue(usuarioRemovido);
    }

    [TestMethod]
    public void RemoverUsuario_Inexistente_RetornaFalso()
    {
        string email = "asdasdasda@gmail.com";

        bool usuarioRemovido = _sistema.EliminarUsuario(email);

        Assert.IsFalse(usuarioRemovido);
    }

    [TestMethod]
    public void RemoverUsuario_EmailInvalido_RetornaFalso()
    {
        string email = "manuel.com";

        bool usuarioRemovido = _sistema.EliminarUsuario(email);

        Assert.IsFalse(usuarioRemovido);
    }

    [TestMethod]
    public void RemoverUsuario_EmailNulo_RetornaFalso()
    {
        string? email = null;

        bool usuarioRemovido = _sistema.EliminarUsuario(email);

        Assert.IsFalse(usuarioRemovido);
    }
    // Fin Pruebas Usuario

    // Inicio Pruebas Items

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaTituloYDescripcion()
    {
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

        _sistema.ActualizarItemEnCatalogo(catalogo, dto);

        Assert.AreEqual("Nuevo Título", item.Titulo);
        Assert.AreEqual("Nueva Descripción", item.Descripcion);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ModificaCategoriaMarcaModelo()
    {

        var item = new Item("Original", "Descripcion original")
        {
            Categoria = "Cat 1",
            Marca = "Marca 1",
            Modelo = "Modelo 1"
        };
        _catalogo.AgregarItem(item);

        var dto = new ItemEditDataTransfer
        {
            Id = item.Id,
            Titulo = "Original",
            Descripcion = "Descripcion original",
            Categoria = "Cat 2",
            Marca = "Marca 2",
            Modelo = "Modelo 2"
        };

        _sistema.ActualizarItemEnCatalogo(_catalogo, dto);


        Assert.AreEqual("Cat 2", item.Categoria);
        Assert.AreEqual("Marca 2", item.Marca);
        Assert.AreEqual("Modelo 2", item.Modelo);
    }

    [TestMethod]
    public void ActualizarItemEnCatalogo_ItemNoExiste_Excepcion()
    {



        var dto = new ItemEditDataTransfer
        {
            Id = 999, // Id inexistente
            Titulo = "Título",
            Descripcion = "Descripcion",
            Categoria = "Cat",
            Marca = "Marca",
            Modelo = "Modelo"
        };

        var ex = Assert.ThrowsException<ItemException>(() => _sistema.ActualizarItemEnCatalogo(_catalogo, dto)
        );

        Assert.AreEqual("No se encontró el item a actualizar.", ex.Message);
    }

    [TestMethod]
    public void AltaItem_AgregaItemAlCatalogo()
    {

        var nuevoItem = new Item("Item 1", "Descripción 1");

        _sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
        var items = _catalogo.Items;

        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Item 1", items.First().Titulo);
        Assert.AreEqual("Descripción 1", items.First().Descripcion);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AltaItem_SinCatalogo_Excepcion()
    {

        var nuevoItem = new Item("Item 1", "Desc");

        _sistema.AltaItemConAltaDuplicados("Inexistente", nuevoItem);

    }

    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_LanzaExcepcionSiTituloOVacio()
    {


        var nuevoItem = new Item("", "Descripción 1");
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }


    [TestMethod]
    [ExpectedException(typeof(ItemException))]
    public void AltaItem_DescripcionVacia_Excepcion()
    {


        var nuevoItem = new Item("Titulo", "");
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", nuevoItem);
    }
    
    [TestMethod]
    public void AltaItemConAltaDuplicados_AgregaItemYGeneraDuplicadoEnListaGlobal()
    {
        var item1 = new Item("Titulo 1", "Descripcion 1");
        var item2 = new Item("Titulo 1", "Descripcion 1"); 

        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item1);
        _sistema.AltaItemConAltaDuplicados("Catálogo Test", item2);

        
        Assert.IsTrue(_sistema.DuplicadosGlobales.Count == 1);
    }

    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarDuplicadosPara_ExcepcionSiCatalogoEsNull()
    {
        var item = new Item("Titulo", "Descripcion");
        _sistema.ActualizarDuplicadosPara(null, item);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ActualizarDuplicadosPara_LanzaExcepcionSiItemEsNull()
    {
        _sistema.ActualizarDuplicadosPara(_catalogo, null);
    }
    [TestMethod]
    public void ActualizarDuplicados_EliminaDuplicadosPreviosDelItem()
    {
        var item1 = new Item("Item 1", "Desc 1");
        var item2 = new Item("Item 2", "Desc 2");

        _sistema.AltaItemConAltaDuplicados("Desc 1",item1);
        _sistema.AltaItemConAltaDuplicados("Desc 2",item2);


        Assert.AreEqual(1, _sistema.DuplicadosGlobales.Count);
        item1.Titulo = "Titulo Editado";
        item1.Descripcion = "Descripcion Editada";
        
        _sistema.ActualizarDuplicadosPara(_catalogo, item1);

        Assert.AreEqual(0, _sistema.DuplicadosGlobales.Count, "Los duplicados previos deberían eliminarse.");
    }
}
    
    
    
    
    
    
