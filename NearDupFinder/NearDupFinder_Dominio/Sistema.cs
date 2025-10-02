using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio;

public class Sistema
{
    private List<Usuario> _usuarios = [];

    public Sistema()
    {
        _usuarios.Add(CrearUsuarioAdmin());
    }

    // USUARIOS
    private Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }
    
    public bool CrearUsuario(string? nombre, string? apellido, string? email, int anio, int mes, int dia, string? clave, List<Rol>? roles)
    {
        Email correo;
        Fecha fecha;
        Usuario nuevoUsuario;
        Contrasena contrasena;
        try
        {
            correo = Email.Crear(email);
            if (BuscarUsuarioPorEmail(correo) is not null)
                return false;

            fecha = Fecha.Crear(anio,mes,dia);
            contrasena = Contrasena.Crear(clave);
            nuevoUsuario = Usuario.Crear(nombre,apellido,correo,fecha);
        }
        catch
        {
            return false;
        }
        
        nuevoUsuario.CambiarContrasena(contrasena);
        if(roles is null)
            return false;
        foreach (var rol in roles)
            nuevoUsuario.AgregarRol(rol);
        _usuarios.Add(nuevoUsuario);
        
        return true;
    }
    
    public List<Usuario> ObtenerUsuarios()
    {
        return _usuarios;
    }

    public Usuario? BuscarUsuarioPorId(int id)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Id == id)
                return usuario;
        return null;    
    }
    
    private Usuario? BuscarUsuarioPorEmail(Email email)
    {
        foreach (Usuario usuario in _usuarios)
            if (usuario.Email.Igual(email))
                return usuario;
        return null;
    }

    // La contraseña es opcional, en caso que no se ingrese no se cambia.
    public bool ModificarUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol>? roles)
    {
        Email correo;
        Fecha fecha;
        Usuario? usuarioAModificar;
        Contrasena? contrasena = null;
        try
        {
            correo = Email.Crear(email);
            usuarioAModificar = BuscarUsuarioPorEmail(correo);
            if(usuarioAModificar is null)
                return false;
            
            fecha = Fecha.Crear(anio,mes,dia);
            if(!string.IsNullOrWhiteSpace(clave))
                contrasena = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || roles is null)
            return false;
        
        usuarioAModificar.Nombre = nombre;
        usuarioAModificar.Apellido = apellido;
        usuarioAModificar.FechaNacimiento = fecha;
        usuarioAModificar.ObtenerRoles().Except(roles).ToList().ForEach(r => usuarioAModificar.RemoverRol(r));
        roles.Except(usuarioAModificar.ObtenerRoles()).ToList().ForEach(r => usuarioAModificar.AgregarRol(r));
        
        if(!string.IsNullOrWhiteSpace(clave))
            usuarioAModificar.CambiarContrasena(contrasena);
        
        return true;
    }
    
    public bool ModificarClave(string? email, string? clave)
    {
        Email correo;
        Contrasena contrasena;
        try
        {
            correo = Email.Crear(email);
            contrasena = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }
        Usuario? usuario = BuscarUsuarioPorEmail(correo);
        if (usuario is null)
            return false;

        return usuario.CambiarContrasena(contrasena);
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
        Usuario? usuario = BuscarUsuarioPorEmail(emailAValidar);
        if (usuario is null)
            return null;
        
        if (usuario.VerificarContrasena(clave))
            return usuario;
        
        return null;
    }

    public bool EliminarUsuario(string? email)
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
        Usuario? usuario = BuscarUsuarioPorEmail(emailUsuario);
        if (usuario is null)
            return false;
        
        _usuarios.Remove(usuario);
        
        return true;
    }
    // FIN USUARIOS
}