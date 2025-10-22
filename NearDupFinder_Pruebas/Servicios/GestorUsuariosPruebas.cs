using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;
using NearDupFinder_LogicaDeNegocio.Servicios;

namespace NearDupFinder_Pruebas.Servicios;

[TestClass]
public class GestorUsuariosPruebas
{/*
    private static Email CrearEmail(string email) => Email.Crear(email);
    private static Fecha CrearFecha(int a, int m, int d) => Fecha.Crear(a, m, d);
    private static Usuario CrearUsuario(
        string nombre = "Manuel",
        string apellido = "Perez",
        string mail = "manuel@ejemplo.com",
        int a = 1997, int m = 12, int d = 27) =>
        Usuario.Crear(nombre, apellido, CrearEmail(mail), CrearFecha(a, m, d));
    
    private Sistema _sistema = null!;
    private Catalogo _catalogo = null!;
    private AlmacenamientoDeDatos _almacenamiento = null!;
    private GestorUsuarios _gestorUsuarios = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
        _almacenamiento = new AlmacenamientoDeDatos();
        _gestorUsuarios = new GestorUsuarios(_sistema, _almacenamiento);
        _catalogo = new Catalogo("Catalogo Test");
        _sistema.AgregarCatalogo(_catalogo);
        
    }
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
        List<Rol> roles = [Rol.Revisor];
        
        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_ClaveVacia_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "";
        List<Rol> roles = [Rol.Revisor];

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_PuedeNoTenerRoles_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<Rol> roles = new List<Rol>();

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsTrue(usuarioCreado);
    }
    
    [TestMethod]
    public void CrearUsuario_UsuarioYaExistente_RetornaFalso()
    {
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
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool usuarioCreado2 = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre2, apellido2, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado2);
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

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsTrue(usuarioCreado);
    }
    
    [TestMethod]
    public void ModificarUsuario_NombreVacio_RetornaFalso()
    {
        string nombre = "Manuel";
        string nombreVacio = "";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailVacio = null;
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombreVacio, apellido, anio, mes, dia, clave, roles));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ApellidoVacio_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string apellidoVacio = "";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombre, apellidoVacio, anio, mes, dia, clave, roles));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_EmailInvalido_RetornaFalso()
    {
        string  nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInvalido = "manuel.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(emailInvalido, nombre, apellido, anio, mes, dia, clave, roles));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_FechaInvalida_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        int diaInvalido = 100;
        int mesInvalido = 13;
        int anioInvalido = 0;
        
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombre, apellido, anioInvalido, mesInvalido, diaInvalido, clave, roles));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_ClaveInvalida_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        string claveInvalida = "Invalida";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, claveInvalida, roles));

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_ClaveVacia_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        string claveVacia = string.Empty;
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, claveVacia, roles) );

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaNombreYApellido_RetornaVerdaderoYActualizaCampos()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string nombreNuevo = "NuevoNombre";
        string apellidoNuevo = "NuevoApellido";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombreNuevo, apellidoNuevo,anio, mes, dia, clave, roles));
        
        var usuario = _gestorUsuarios.AutenticarUsuario(email, clave);
        
        Assert.IsTrue(modificado);
        Assert.AreEqual(nombreNuevo, usuario!.Nombre);
        Assert.AreEqual(apellidoNuevo, usuario.Apellido);
    }

    [TestMethod]
    public void ModificarUsuario_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, clave, roles));
        Usuario? usuarioAutenticado = _gestorUsuarios.AutenticarUsuario(email,clave);

        Assert.IsTrue(modificado);
        Assert.IsNotNull(usuarioAutenticado);
    }

    [TestMethod]
    public void ModificarUsuario_RemplazaRoles_SoloQuedaListaNueva()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombre, apellido, anio, mes, dia, clave, roles));
        
        var usuario = _gestorUsuarios.AutenticarUsuario(email, clave);
        
        var rolesUsuario = usuario!.ObtenerRoles().ToList();
        
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
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        List<Rol> rolesVacios = [];
        
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombre, apellido , anio, mes, dia, clave, rolesVacios));
        var usuario = _gestorUsuarios.AutenticarUsuario(email, clave);
        var rolesUsuario = usuario!.ObtenerRoles().ToList();

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
        string clave = "123QWEasdzxc@";
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido,anio, mes, dia, clave, roles));
        
        Assert.IsTrue(modificado);
    }
    
    [TestMethod]
    public void ModificarUsuario_UsuarioInexistente_RetornaFalso()
    {
        string nombre = "Manu";
        string apellido = "Pérez";
        string email = "noexiste@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];

        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, clave, roles));

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        
        bool usuarioRemovido = _gestorUsuarios.BorrarUsuario(new DatosUsuarioEmail(email));
        
        Assert.IsTrue(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_Inexistente_RetornaFalso()
    {
        string email = "asdasdasda@gmail.com";
        
        bool usuarioRemovido = _gestorUsuarios.BorrarUsuario(new DatosUsuarioEmail(email));
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_EmailInvalido_LanzaExcepcion()
    {
        string email = "manuel.com"; 
        var datos = new DatosUsuarioEmail(email);

        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.BorrarUsuario(datos)
        );

        StringAssert.Contains(ex.Message, "El email no tiene un formato válido.");
    }
    
    [TestMethod]
    public void ModificarClave_CamposValidos_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        string claveActual = "123QWEasdzxc@";
        
        string claveNueva = "NuevaClaveValida123!";
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email,claveActual, claveNueva));

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
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));

        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(emailInexistente, claveActual, claveNueva));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInexistente = "noexiste@.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(emailInexistente, claveActual, claveNueva));
        

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "invalida";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        
        
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        
        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarClave_ClaveActualInCorrecta_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveActualInvalida = "NuevaClaveValida123!";
        string claveNueva = "Encr1pt4d0@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];

        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActualInvalida, claveNueva));


        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_PermiteAutenticarConNuevaClave()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "Encr1pt4d0@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        Usuario? admin = _gestorUsuarios.AutenticarUsuario(email, claveNueva);

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
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "Encr1pt4d0@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));

        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        Usuario? admin = _gestorUsuarios.AutenticarUsuario(email, claveActual);

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
        string clave = "123QWEasdzxc@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        Usuario? admin = _gestorUsuarios.AutenticarUsuario(email, clave);
        
        Assert.IsNotNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string clave = "123QWEasdzxc@";
        string claveIncorrecta = "Incorrecta@!";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        Usuario? admin = _gestorUsuarios.AutenticarUsuario(email, claveIncorrecta);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailIncorrecto_RetornaNulo()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string clave = "123QWEasdzxc@";
        string emailIncorrecto = "incorrecto@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<Rol> roles = [Rol.Revisor];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        Usuario? admin = _gestorUsuarios.AutenticarUsuario(emailIncorrecto, clave);
        
        Assert.IsNull(admin);
    }
    
    [TestMethod]
    public void UsuarioTieneRol_TieneRolRevisor_RetornaVerdadero()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);

        Assert.IsTrue(_gestorUsuarios.UsuarioTieneRol(usuario, Rol.Revisor));
    }
    
    [TestMethod]
    public void UsuarioTieneRol_NoTieneRolRevisor_RetornaFalso()
    {
        var usuario = CrearUsuario();
        
        Assert.IsFalse(_gestorUsuarios.UsuarioTieneRol(usuario, Rol.Revisor));
    }
    
    [TestMethod]
    public void ObtenerRolesDeUsuario_TieneRolRevisor_RetornaListaConRolRevisor()
    {
        var usuario = CrearUsuario();

        usuario.AgregarRol(Rol.Revisor);
        var listaDeRolesDelUsuario = _gestorUsuarios.ObtenerRolesDeUsuario(usuario);
        
        Assert.AreEqual(1, listaDeRolesDelUsuario.Count());
        Assert.IsTrue(usuario.TieneRol(Rol.Revisor));
    }
    
    [TestMethod]
    public void ObtenerUsuarios_DevuelveUsuariosExistentes()
    {
        var usuarios = _gestorUsuarios.ObtenerUsuarios();

        Assert.IsNotNull(usuarios);
        Assert.AreEqual(1, usuarios.Count);
        
    }

    [TestMethod]
    public void ObtenerUsuarios_SinUsuarios_DevuelveListaVacia()
    {
        _gestorUsuarios.BorrarUsuario(new DatosUsuarioEmail("admin@gmail.com"));
        var usuarios = _gestorUsuarios.ObtenerUsuarios();
        
        Assert.IsNotNull(usuarios);
        Assert.AreEqual(0, usuarios.Count);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorId_UsuarioExistente_RetornaUsuario()
    {
        
        var usuarios = _gestorUsuarios.ObtenerUsuarios();
        
        int idAdmin = usuarios.First().Id;
        
        var usuario = _gestorUsuarios.BuscarUsuarioPorId(idAdmin);
        
        Assert.IsNotNull(usuario);
        Assert.AreEqual(idAdmin, usuario!.Id);
    }

    [TestMethod]
    public void BuscarUsuarioPorId_UsuarioInexistente_RetornaNull()
    {
        int idInexistente = 9999;
      
        var usuario = _gestorUsuarios.BuscarUsuarioPorId(idInexistente);
        
        Assert.IsNull(usuario);
    }
*/
}