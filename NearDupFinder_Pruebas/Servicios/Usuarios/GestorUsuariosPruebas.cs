using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaLogin;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Usuarios;

[TestClass]
public class GestorUsuariosPruebas
{
    private static DatosRegistroUsuario CrearDtoUsuario()
    {
        return new DatosRegistroUsuario(
            "Manuel", 
            "Perez",
            "manuel@gmail.com",
            1997,
            12,
            27,
            "123QWEasdzxc@",
            ["Administrador"]);
    }
    
    private AlmacenamientoDeDatos _almacenamiento = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private GestorUsuarios _gestorUsuarios = null!;
    private GestorAutenticacionUsuario _gestorAutenticacionUsuario = null!;

    
    [TestInitialize]
    public void Setup()
    {
        _almacenamiento = new AlmacenamientoDeDatos();
        _gestorAuditoria = new GestorAuditoria();
        _gestorAutenticacionUsuario = new GestorAutenticacionUsuario(_almacenamiento);
        _gestorUsuarios = new GestorUsuarios(_almacenamiento, _gestorAuditoria,_gestorAutenticacionUsuario);
        _gestorAuditoria.AsignarUsuarioActual("manuel@gmail.com");
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
        List<string> roles = ["Revisor"];
        
        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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
        List<string> roles = ["Revisor"];

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

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
        List<string> roles = ["Revisor"];

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_FechaInvalida_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuel@gmail.com";
        int anio = 1997;
        int mes = 15;
        int dia = 49;
        string clave = "ClaveValida123!";
        List<string> roles = ["Revisor"];

        bool usuarioCreado = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void CrearUsuario_ClaveInvalido_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveInvalida";
        List<string> roles = ["Revisor"];

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
        List<string> roles = ["Revisor"];

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
        List<string> roles = [];

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
        List<string> roles = ["Administrador"];
        string nombre2 = "Juan";
        string apellido2 = "Perez";
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool usuarioCreado2 = _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre2, apellido2, email, anio, mes, dia, clave, roles));

        Assert.IsFalse(usuarioCreado2);
    }
    
    [TestMethod]
    public void CrearUsuario_CamposValidos_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        string clave = "ClaveValida123!";
        List<string> roles = ["Administrador"];

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
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "ClaveValida123!";
        List<string> roles = ["Revisor"];
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
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombreNuevo, apellidoNuevo,anio, mes, dia, clave, roles));
        var usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(new DatosAutenticacion(email,clave));
        
        Assert.IsTrue(modificado);
        Assert.AreEqual(nombreNuevo, usuarioAutenticado!.Nombre);
        Assert.AreEqual(apellidoNuevo, usuarioAutenticado.Apellido);
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
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, clave, roles));
        var usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(new DatosAutenticacion(email,clave));

        Assert.IsTrue(modificado);
        Assert.IsNotNull(usuarioAutenticado);
    }

    [TestMethod]
    public void ModificarUsuario_RolesVacio_EliminaTodosLosRoles()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "123QWEasdzxc@";
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        List<string> rolesVacios = [];
        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email, nombre, apellido , anio, mes, dia, clave, rolesVacios));
        var usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(new DatosAutenticacion(email,clave));
        var rolesUsuario = usuarioAutenticado!.ObtenerRoles().ToList();

        Assert.IsTrue(modificado);
        Assert.AreEqual(0, rolesUsuario.Count);
    }
    
    [TestMethod]
    public void ModificarUsuario_CamposValidos_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1995;
        int mes = 5;
        int dia = 10;
        string clave = "123QWEasdzxc@";
        List<string> roles = ["Revisor"];
        
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
        List<string> roles = ["Revisor"];

        bool modificado = _gestorUsuarios.ModificarUsuario(new DatosEdicionUsuario(email,nombre, apellido, anio, mes, dia, clave, roles));

        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string clave = "ClaveValida123!";
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, clave, roles));
        bool usuarioRemovido = _gestorUsuarios.BorrarUsuario(email);
        
        Assert.IsTrue(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_Inexistente_RetornaFalso()
    {
        string email = "asdasdasda@gmail.com";
        
        bool usuarioRemovido = _gestorUsuarios.BorrarUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_EmailInvalido_LanzaExcepcion()
    {
        string email = "manuel.com";

        bool seBorroElUsuario = _gestorUsuarios.BorrarUsuario(email);
        
        Assert.IsFalse(seBorroElUsuario);
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
        List<string> roles = ["Revisor"];
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email,claveActual, claveNueva));

        Assert.IsTrue(modificado);
    }

    [TestMethod]
    public void ModificarClave_UsuarioInexistente_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInexistente = "noexiste@gmail.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(emailInexistente, claveActual, claveNueva));

        Assert.IsFalse(modificado);
    }

    [TestMethod]
    public void ModificarClave_EmailInvalido_LanzaExcepcion()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string emailInexistente = "noexiste@.com";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<string> roles = ["Revisor"];
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "NuevaClaveValida123!";

        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.ModificarClave(new DatosCambioClave(emailInexistente, claveActual, claveNueva))
        );

        StringAssert.Contains(ex.Message, "El email no es valido");    
    }
    

    [TestMethod]
    public void ModificarClave_ClaveInvalida_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "invalida";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        
        Assert.IsFalse(modificado);
    }
    
    [TestMethod]
    public void ModificarClave_ClaveActualInCorrecta_RetornaFalso()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveActualInvalida = "NuevaClaveValida123!";
        string claveNueva = "Encr1pt4d0@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<string> roles = ["Revisor"];

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
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        var usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(new DatosAutenticacion(email,claveNueva));

        Assert.IsTrue(modificado);
        Assert.IsNotNull(usuarioAutenticado);
    }

    [TestMethod]
    public void ModificarClave_CambiaClave_NoPermiteAutenticarConClaveVieja()
    {
        string nombre = "Manuel";
        string apellido = "Pérez";
        string email = "manuel@gmail.com";
        string claveActual = "123QWEasdzxc@";
        string claveNueva = "Encr1pt4d0@";
        int anio = 1990;
        int mes = 1;
        int dia = 1;
        List<string> roles = ["Revisor"];
        
        _gestorUsuarios.CrearUsuario(new DatosRegistroUsuario(nombre, apellido, email, anio, mes, dia, claveActual, roles));
        bool modificado = _gestorUsuarios.ModificarClave(new DatosCambioClave(email, claveActual, claveNueva));
        var usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(new DatosAutenticacion(email,claveActual));

        Assert.IsTrue(modificado);
        Assert.IsNull(usuarioAutenticado);
    }
    
    [TestMethod]
    public void UsuarioTieneRol_TieneRolAdministrador_RetornaVerdadero()
    {
        var usuarioDto = CrearDtoUsuario();
        _gestorUsuarios.CrearUsuario(usuarioDto);
        
        Assert.IsTrue(_gestorUsuarios.UsuarioTieneRol(usuarioDto.Email!, "Administrador"));
    }
    
    [TestMethod]
    public void UsuarioTieneRol_NoTieneRolRevisor_RetornaFalso()
    {
        var usuarioDto = CrearDtoUsuario();
        _gestorUsuarios.CrearUsuario(usuarioDto);
        
        Assert.IsFalse(_gestorUsuarios.UsuarioTieneRol(usuarioDto.Email!, "Revisor"));
    }
    
    [TestMethod]
    public void UsuarioTieneRol_EmailInvalido_LanzaExcepcion()
    {
        string emailInvalido = "invalido.com";
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.UsuarioTieneRol(emailInvalido, "Revisor")
        );

        StringAssert.Contains(ex.Message, "El email no es valido");   
    }
    
    [TestMethod]
    public void UsuarioTieneRol_UsuarioInexistente_LanzaExcepcion()
    {
        string emailInexistente = "inexistente@gmail.com";
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.UsuarioTieneRol(emailInexistente, "Revisor")
        );

        StringAssert.Contains(ex.Message, "El usuario no existe");    
    }
    
    [TestMethod]
    public void ObtenerUsuarios_DevuelveUsuariosExistentes()
    {
        var usuarioDto = CrearDtoUsuario();
        _gestorUsuarios.CrearUsuario(usuarioDto);
        
        IReadOnlyList<DatosPublicosUsuario> usuarios = _gestorUsuarios.ObtenerUsuarios();

        Assert.AreNotEqual(0, usuarios.Count);
    }

    [TestMethod]
    public void ObtenerUsuarios_SinUsuarios_DevuelveListaVacia()
    {
        string email = "admin@gmail.com";
        _gestorUsuarios.BorrarUsuario(email);
        IReadOnlyList<DatosPublicosUsuario> usuarios = _gestorUsuarios.ObtenerUsuarios();
        
        Assert.AreEqual(0, usuarios.Count);
    }
    
    [TestMethod]
    public void ObtenerIdDeUsuario_UsuarioExistente_DevuelveId()
    {
        var usuarioDto = CrearDtoUsuario();
        _gestorUsuarios.CrearUsuario(usuarioDto);
        
        int idUsuario = _gestorUsuarios.ObtenerIdDeUsuario(usuarioDto.Email!);
        
        Assert.IsNotNull(idUsuario);
    }
    
    [TestMethod]
    public void ObtenerIdDeUsuario_UsuarioInexistente_LanzaExcepcion()
    {
        string emailInexistente = "inexistente@gmail.com";
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.ObtenerIdDeUsuario(emailInexistente)
        );

        StringAssert.Contains(ex.Message, "El usuario no existe");  
    }
    
    [TestMethod]
    public void ObtenerIdDeUsuario_EmailInvalido_LanzaExcepcion()
    {
        string emailInvalido = "invalido.com";
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.ObtenerIdDeUsuario(emailInvalido)
        );

        StringAssert.Contains(ex.Message, "El email no es valido");  
    }
    
    [TestMethod]
    public void ObtenerUsuarioPorId_UsuarioExistente_RetornaUsuario()
    {
        var usuarioDto = CrearDtoUsuario();
        _gestorUsuarios.CrearUsuario(usuarioDto);
        
        int idUsuario = _gestorUsuarios.ObtenerIdDeUsuario(usuarioDto.Email!);
        DatosPublicosUsuario usuario = _gestorUsuarios.ObtenerUsuarioPorId(idUsuario);
        
        Assert.AreEqual(usuario.Email,usuarioDto.Email);
    }

    [TestMethod]
    public void ObtenerUsuarioPorId_UsuarioInexistente_LanzaExcepcion()
    {
        const int idInexistente = 9999;
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorUsuarios.ObtenerUsuarioPorId(idInexistente)
        );

        StringAssert.Contains(ex.Message, "El usuario no existe");    
    }
}