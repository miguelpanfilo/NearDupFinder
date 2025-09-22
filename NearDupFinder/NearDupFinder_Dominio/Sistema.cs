using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio;

public class Sistema
{
    private List<Usuario> _usuarios = [];

    public Sistema()
    {
        _usuarios.Add(CrearUsuarioAdmin());
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

        foreach (var usuario in _usuarios)
        {
            if (usuario.Email.Igual(emailAValidar) && usuario.VerificarContrasena(clave))
                return usuario;
        }
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
        catch (Exception e)
        {
            return false;
        }
        _usuarios.Add(nuevoUsuario);
        return true;
    }
}