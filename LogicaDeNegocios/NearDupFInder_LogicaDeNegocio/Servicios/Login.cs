using NearDupFinder_Almacenamiento;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class Login
{
    private readonly AlmacenamientoDeDatos _almacenamiento;
    
    public Login(AlmacenamientoDeDatos almacenamiento)
    {
        _almacenamiento = almacenamiento;
    }

    public Usuario? AutenticarUsuario(DatosAutenticacion datosAutenticaciones)
    {
        try
        {
            var emailAValidar = Email.Crear(datosAutenticaciones.Email);
            Usuario? usuario = _almacenamiento.BuscarUsuarioPorEmail(emailAValidar);
            if (usuario is null)
                return null;

            return usuario.VerificarClave(datosAutenticaciones.Clave) ? usuario : null;
        }
        catch (ExcepcionDeUsuario e){
        
             throw e;
        }
    }
}