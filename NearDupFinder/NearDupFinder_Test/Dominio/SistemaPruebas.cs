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
}