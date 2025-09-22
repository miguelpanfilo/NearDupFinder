using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio;

public class Sistema
{
    private List<Usuario> _usuarios = [];

    public Sistema()
    {
        _usuarios.Add(CrearUsuarioAdmin());
        CrearUsuariosDebug();
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _usuarios;
    }

    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }

    private void CrearUsuariosDebug()
    {
        Email email1 = Email.Crear("manuel@gmail.com");
        Fecha fecha1 = Fecha.Crear(1997,12,27);
        Email email2 = Email.Crear("Vale@gmail.com");
        Fecha fecha2 = Fecha.Crear(1998,11,7);
        Email email3 = Email.Crear("juan@gmail.com");
        Fecha fecha3 = Fecha.Crear(2000,12,18);
        Usuario usu1 = Usuario.Crear("Manuel","Perez",email1,fecha1);
        Usuario usu2 = Usuario.Crear("Valeria","Sarro",email2,fecha2);
        Usuario usu3 = Usuario.Crear("Juan","Perez",email3,fecha3);
        usu1.AgregarRol(Rol.Administrador);
        usu1.AgregarRol(Rol.Revisor);
        usu2.AgregarRol(Rol.Revisor);
        
        _usuarios.Add(usu1);
        _usuarios.Add(usu2);
        _usuarios.Add(usu3);
    }

    private Usuario? BuscarUsuario(Email email)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Email.Igual(email))
                return usuario;
        return null;
    }

    public Usuario? AutenticarUsuario(string? email, string? clave)
    {
        Email emailAValidar;
        try
        {
            emailAValidar = Email.Crear(email);
        }
        catch
        {
            return null;
        }
        Usuario? usuario = BuscarUsuario(emailAValidar);
        if (usuario == null)
            return null;
        
        if (usuario.VerificarContrasena(clave))
            return usuario;
        
        return null;
    }
    
    public bool CrearUsuario(string? nombre, string? apellido, string? email, int anio, int mes, int dia)
    {
        Email correo;
        Fecha fecha;
        Usuario nuevoUsuario;
        try
        {
            correo = Email.Crear(email);
            fecha = Fecha.Crear(anio,mes,dia);
            nuevoUsuario = Usuario.Crear(nombre,apellido,correo,fecha);
        }
        catch
        {
            return false;
        }
        _usuarios.Add(nuevoUsuario);
        return true;
    }

    public bool RemoverUsuario(string? email)
    {
        Email emailUsuario;
        try
        {
            emailUsuario = Email.Crear(email);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuario(emailUsuario);
        if (usuario is null)
            return false;
        
        _usuarios.Remove(usuario);
        return true;
    }

    public bool CambiarClave(string? correo, string? clave)
    {
        Email email;
        Contrasena contrasena;
        try
        {
            email = Email.Crear(correo);
            contrasena = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuario(email);
        if (usuario is null)
            return false;
        
        usuario.CambiarContrasena(contrasena);
        return true;
    }

    public bool ResetClave(string? email)
    {
        Email correo;
        try
        {
            correo = Email.Crear(email);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuario(correo);
        if (usuario is null)
            return false;
        
        usuario.ResetearContrasena();
        return true;
    }
}