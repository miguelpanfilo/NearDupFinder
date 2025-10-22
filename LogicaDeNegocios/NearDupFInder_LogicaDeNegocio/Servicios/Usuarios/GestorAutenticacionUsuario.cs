using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaLogin;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

public class GestorAutenticacionUsuario(AlmacenamientoDeDatos almacenamiento)
{
    public Usuario? AutenticarUsuario(DatosAutenticacion datosAutenticacion)
    {
        Email? emailAValidar;
        try
        {
            emailAValidar = Email.Crear(datosAutenticacion.Email);
        }
        catch
        {
            throw new ExcepcionDeUsuario("El email no es valido");
        }
        
        Usuario? usuario = almacenamiento.BuscarUsuarioPorEmail(emailAValidar);
        
        if (usuario is null)
            return null;

        return usuario.VerificarClave(datosAutenticacion.Clave) ? usuario : null;
    }
    
    public bool AutenticarUsuarioBooleano(DatosAutenticacion datosAutenticacion)
    {
        Usuario? usuario = AutenticarUsuario(datosAutenticacion);
        if (usuario is null)
            return false;
        
        return true;
    }
    
    public DatosIdentificacion AutenticarUsuarioDto(DatosAutenticacion datosAutenticacion)
    {
        Usuario? usuario = AutenticarUsuario(datosAutenticacion);;
        if (usuario is null)
            throw new ExcepcionDeUsuario("El usuario no es valido");

        return DatosIdentificacion.FromEntity(usuario);
    }
}