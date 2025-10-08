using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Dominio.Controladores;

public class GestorUsuarios(Sistema sistema)
{
    public Usuario CrearUsuarioAdmin()
    {
        Email email = Email.Crear("admin@gmail.com");
        Fecha fecha = Fecha.Crear(1997,12,27);
        Contrasena contrasena = Contrasena.Crear("123QWEasdzxc@");
        Usuario adminUsuario = Usuario.Crear("admin","admin",email,fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        adminUsuario.CambiarContrasena(contrasena);
        
        return adminUsuario;
    }
    
    public bool CrearUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string clave, List<Rol> roles)
    {
        Usuario nuevoUsuario;
        Contrasena claveDelUsuario;
        try
        {
            Email correo = Email.Crear(email);
            if (ElEmailYaEstaRegistrado(correo))
                return false;

            Fecha fecha = Fecha.Crear(anio,mes,dia);
            claveDelUsuario = Contrasena.Crear(clave);
            nuevoUsuario = Usuario.Crear(nombre,apellido,correo,fecha);
        }
        catch
        {
            return false;
        }

        CambiarClaveDelUsuarioSiCorresponde(nuevoUsuario, claveDelUsuario);
        AgregarRolesAlUsuario(nuevoUsuario, roles);
        AgregarALaListaDeUsuarios(nuevoUsuario);
        
        return true;
    }
    
    public bool EditarDatosDelUsuario(string nombre, string apellido, string email, int anio, int mes, int dia, string? clave, List<Rol> roles)
    {
        if (!ValidarNombreYApellido(nombre, apellido))
            return false;
        
        Usuario? usuarioAModificar;
        Contrasena? posibleNuevaClave = null;
        Fecha fecha;
        try
        {
            Email correo = Email.Crear(email);
            usuarioAModificar = BuscarUsuarioPorEmailEnSistema(correo);
            if(usuarioAModificar is null)
                return false;
            
            fecha = Fecha.Crear(anio,mes,dia);
            
            if(!string.IsNullOrWhiteSpace(clave))
                posibleNuevaClave = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }
        
        ModificarUsuarioExistente(usuarioAModificar, nombre, apellido, fecha, roles);
        CambiarClaveDelUsuarioSiCorresponde(usuarioAModificar, posibleNuevaClave);
        
        return true;
    }
    
    public bool BorrarUsuario(string email)
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
        
        Usuario? usuario = BuscarUsuarioPorEmailEnSistema(emailUsuario);
        if (usuario is null)
            return false;
        
        RemoverDeLaListaDeUsuarios(usuario);
        
        return true;
    }
    
    public bool ModificarClave(Usuario usuario, string? clave)
    {
        Contrasena nuevaClave;
        try
        {
            nuevaClave = Contrasena.Crear(clave);
        }
        catch
        {
            return false;
        }

        CambiarClaveDelUsuarioSiCorresponde(usuario, nuevaClave);
        
        return true;
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
        Usuario? usuario = BuscarUsuarioPorEmailEnSistema(emailAValidar);
        if (usuario is null)
            return null;
        
        if (usuario.VerificarContrasena(clave))
            return usuario;
        
        return null;
    }

    private bool ElEmailYaEstaRegistrado(Email email)
    {
        if (sistema.BuscarUsuarioPorEmail(email) is not null)
            return true;
        return false;
    }

    private Usuario? BuscarUsuarioPorEmailEnSistema(Email email)
    {
        return sistema.BuscarUsuarioPorEmail(email);
    }

    private void CambiarClaveDelUsuarioSiCorresponde(Usuario usuario, Contrasena? clave)
    {
        if(clave is not null)
            usuario.CambiarContrasena(clave);
    }

    private void AgregarRolesAlUsuario(Usuario usuario, List<Rol> roles)
    {
        foreach (var rol in roles)
            usuario.AgregarRol(rol);
    }

    private void AgregarALaListaDeUsuarios(Usuario usuario)
    {
        sistema.AgregarUsuarioDeLaLista(usuario);
    }

    private void RemoverDeLaListaDeUsuarios(Usuario usuario)
    {
        sistema.RemoverUsuarioDeLaLista(usuario);
    }

    private bool ValidarNombreYApellido(string nombre, string apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            return false;
        return true;
    }

    private void ModificarUsuarioExistente(Usuario usuario, string nuevoNombre, string nuevoApellido, Fecha nuevaFecha, List<Rol> nuevosRoles)
    {
        usuario.Nombre = nuevoNombre;
        usuario.Apellido = nuevoApellido;
        usuario.FechaNacimiento = nuevaFecha;
        usuario.ObtenerRoles().Except(nuevosRoles).ToList().ForEach(usuario.RemoverRol);
        nuevosRoles.Except(usuario.ObtenerRoles()).ToList().ForEach(usuario.AgregarRol);
    }

}