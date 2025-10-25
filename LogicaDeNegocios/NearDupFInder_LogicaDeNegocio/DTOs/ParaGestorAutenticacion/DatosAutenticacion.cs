namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaLogin;

public class DatosAutenticacion(string email, string clave)
{
    public string Email { get; } = email;
    public string Clave { get;} = clave;
}