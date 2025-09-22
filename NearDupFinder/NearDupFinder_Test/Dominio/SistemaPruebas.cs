using NearDupFinder_Dominio;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio;

[TestClass]
public class SistemaPruebas
{
    [TestMethod]
    public void AutenticarUsuario_Correto_RetornaUsuario()
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
    public void CrearUsuario_DatosValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string nombre = "Manuel";
        string apellido = "Perez";
        string email = "manuelperezmartirene@gmail.com";
        int anio = 1997;
        int mes = 12;
        int dia = 27;
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
        Assert.IsTrue(usuarioCreado);
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
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
        
        bool usuarioCreado = sistema.CrearUsuario(nombre,apellido,email,anio,mes,dia);
        
        Assert.IsFalse(usuarioCreado);
    }

    [TestMethod]
    public void RemoverUsuario_Existente_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string email = "admin@gmail.com";

        // La clase sistema comienza con el Usuario admin por defecto por lo tanto no es necesario depender de agregar 
        // a un usuario para este test
        bool usuarioRemovido = sistema.RemoverUsuario(email);
        
        Assert.IsTrue(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_Inexistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "manuel@gmail.com";
        
        bool usuarioRemovido = sistema.RemoverUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string email = "manuel.com";
        
        bool usuarioRemovido = sistema.RemoverUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void RemoverUsuario_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? email = null;
        
        bool usuarioRemovido = sistema.RemoverUsuario(email);
        
        Assert.IsFalse(usuarioRemovido);
    }
    
    [TestMethod]
    public void CambiarClave_DatosValidos_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string emailValido = "admin@gmail.com";
        string claveValida = "@Manuel182@";
        
        bool seModificoLaClave = sistema.CambiarClave(emailValido, claveValida);
        
        Assert.IsTrue(seModificoLaClave);
    }
    
    [TestMethod]
    public void CambiarClave_EmailNoExistente_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailNoExistente = "manuek@gmail.com";
        string claveValida = "@Manuel182@";
        
        bool seModificoLaClave = sistema.CambiarClave(emailNoExistente, claveValida);
        
        Assert.IsFalse(seModificoLaClave);
    }
    
    [TestMethod]
    public void CambiarClave_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailInvalido = "manuel.com";
        string claveValida = "@Manuel182@";
        
        bool seModificoLaClave = sistema.CambiarClave(emailInvalido, claveValida);
        
        Assert.IsFalse(seModificoLaClave);
    }
    
    [TestMethod]
    public void CambiarClave_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? emailInvalido = null;
        string claveValida = "@Manuel182@";
        
        bool seModificoLaClave = sistema.CambiarClave(emailInvalido, claveValida);
        
        Assert.IsFalse(seModificoLaClave);
    }
    
    [TestMethod]
    public void CambiarClave_ClaveInvalida_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailValido = "admin@gmail.com";
        string claveInvalida = "invalida";
        
        bool seModificoLaClave = sistema.CambiarClave(emailValido, claveInvalida);
        
        Assert.IsFalse(seModificoLaClave);
    }
    
    [TestMethod]
    public void CambiarClave_ClaveNula_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailValido = "admin@gmail.com";
        string? claveInvalida = null;
        
        bool seModificoLaClave = sistema.CambiarClave(emailValido, claveInvalida);
        
        Assert.IsFalse(seModificoLaClave);
    }

    [TestMethod]
    public void ResetClave_UsuarioValido_RetornaVerdadero()
    {
        Sistema sistema = new Sistema();
        string emailValido = "admin@gmail.com";

        bool seReseteoLaClave = sistema.ResetClave(emailValido);
        
        Assert.IsTrue(seReseteoLaClave);
    }
    
    [TestMethod]
    public void ResetClave_UsuarioNoRegistrado_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailNoRegistrado = "manuel@gmail.com";

        bool seReseteoLaClave = sistema.ResetClave(emailNoRegistrado);
        
        Assert.IsFalse(seReseteoLaClave);
    }
    
    [TestMethod]
    public void ResetClave_EmailInvalido_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string emailInvalido = "manuel.com";

        bool seReseteoLaClave = sistema.ResetClave(emailInvalido);
        
        Assert.IsFalse(seReseteoLaClave);
    }
    
    [TestMethod]
    public void ResetClave_EmailNulo_RetornaFalso()
    {
        Sistema sistema = new Sistema();
        string? emailNulo = null;

        bool seReseteoLaClave = sistema.ResetClave(emailNulo);
        
        Assert.IsFalse(seReseteoLaClave);
    }
}