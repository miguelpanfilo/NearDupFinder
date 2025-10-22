using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaLogin;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFinder_Pruebas.Servicios.Usuarios;

[TestClass]
public class GestorAutenticacionUsuarioPruebas
{
    private GestorAutenticacionUsuario _gestorAutenticacionUsuario = null!;
    private AlmacenamientoDeDatos _almacenamiento = null!;
    private GestorUsuarios _gestorUsuarios = null!;
    private GestorAuditoria _gestorAuditoria = null!;

    private static DatosRegistroUsuario CrearDtoUsuario(string email, string clave)
    {
        return new DatosRegistroUsuario(
            "Manuel", 
            "Perez",
            email,
            1997,
            12,
            27,
            clave,
            ["Administrador", "Revisor"]);
    }

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
    public void AutenticoUsuario_CredencialesValidas_RetornaUsuario()
    {
        string email = "manuel@ejemplo.com"; 
        string clave = "123QWEasdzxc@";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, clave));
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        Usuario? usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(datos);
        
        Assert.AreEqual(email, usuarioAutenticado!.Email.ToString());
    }
    
    [TestMethod]
    public void AutenticoUsuario_UsuarioInexistente_RetornaNulo()
    {
        string email = "manuel@ejemplo.com"; 
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        Usuario? usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(datos);
        
        Assert.IsNull(usuarioAutenticado);
    }
    
    [TestMethod]
    public void AutenticoUsuario_ClaveIncorrecta_RetornaNulo()
    {
        string email = "manuel@ejemplo.com"; 
        string claveIncorrecta = "incorrecta";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, claveIncorrecta));
        DatosAutenticacion datos = new DatosAutenticacion(email, claveIncorrecta);
        
        Usuario? usuarioAutenticado = _gestorAutenticacionUsuario.AutenticarUsuario(datos);
        
        Assert.IsNull(usuarioAutenticado);
    }
    
    [TestMethod]
    public void AutenticoUsuario_EmailInvalido_LanzaExcepcion()
    {
        string email = "manuel@.com"; 
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
                _gestorAutenticacionUsuario.AutenticarUsuario(datos)
        );

        StringAssert.Contains(ex.Message, "El email no es valido");    
    }
    
    [TestMethod]
    public void AutenticarUsuarioBooleano_CredencialesValidas_RetornaVerdadero()
    {
        
        string email = "manuel@ejemplo.com"; 
        string clave = "123QWEasdzxc@";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, clave));
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        bool usuarioSeAutentico = _gestorAutenticacionUsuario.AutenticarUsuarioBooleano(datos);
        
        Assert.IsTrue(usuarioSeAutentico);
    }
    
    [TestMethod]
    public void AutenticarUsuarioBooleano_UsuarioInexistente_RetornaFalso()
    {
        string email = "noexiste@neardupfinder.com";
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        bool usuarioSeAutentico = _gestorAutenticacionUsuario.AutenticarUsuarioBooleano(datos);

        Assert.IsFalse(usuarioSeAutentico);
    }

    [TestMethod]
    public void AutenticarUsuarioBooleano_ClaveIncorrecta_RetornaFalso()
    {
        string email = "manuel@ejemplo.com";
        string claveCorrecta = "123QWEasdzxc@";
        string claveIncorrecta = "ClaveMala123@";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, claveCorrecta));
        DatosAutenticacion datos = new DatosAutenticacion(email, claveIncorrecta);
        
        bool usuarioSeAutentico = _gestorAutenticacionUsuario.AutenticarUsuarioBooleano(datos);

        Assert.IsFalse(usuarioSeAutentico);
    }

    [TestMethod]
    public void AutenticarUsuarioBooleano_EmailInvalido_LanzaExcepcion()
    {
        string emailIncorrecto = "admin@@neardupfinder";
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(emailIncorrecto, clave);

        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorAutenticacionUsuario.AutenticarUsuarioBooleano(datos)
        );

        StringAssert.Contains(ex.Message, "El email no es valido");
    }
    
    [TestMethod]
    public void AutenticarUsuarioDto_CredencialesValidas_EmailCorrecto()
    {
        string email = "manuel@ejemplo.com"; 
        string clave = "123QWEasdzxc@";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, clave));
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
        
        DatosIdentificacion usuario = _gestorAutenticacionUsuario.AutenticarUsuarioDto(datos);
        
        Assert.AreEqual(usuario.Email, email);
    }
    
    [TestMethod]
    public void AutenticarUsuarioDto_UsuarioInexistente_LanzaExcepcion()
    {
        string email = "noexiste@neardupfinder.com";
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(email, clave);
       
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorAutenticacionUsuario.AutenticarUsuarioDto(datos)
        );

        StringAssert.Contains(ex.Message, "El usuario no es valido");
    }

    [TestMethod]
    public void AutenticarUsuarioDto_ClaveIncorrecta_LanzaExcepcion()
    {
        string email = "manuel@ejemplo.com"; 
        string claveCorrecta = "123QWEasdzxc@";
        string claveIncorrecta = "ClaveMala123@";
        _gestorUsuarios.CrearUsuario(CrearDtoUsuario(email, claveCorrecta));
        DatosAutenticacion datos = new DatosAutenticacion(email, claveIncorrecta);
       
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorAutenticacionUsuario.AutenticarUsuarioDto(datos)
        );

        StringAssert.Contains(ex.Message, "El usuario no es valido");
    }

    [TestMethod]
    public void AutenticarUsuarioDto_EmailInvalido_LanzaExcepcion()
    {
        string emailIncorrecto = "admin@@neardupfinder";
        string clave = "123QWEasdzxc@";
        DatosAutenticacion datos = new DatosAutenticacion(emailIncorrecto, clave);
       
        var ex = Assert.ThrowsException<ExcepcionDeUsuario>(() =>
            _gestorAutenticacionUsuario.AutenticarUsuarioDto(datos)
        );

        StringAssert.Contains(ex.Message, "El email no es valido");
    }
}