namespace NearDupFinder_LogicaDeNegocio.Recursos;

public class DatosAutenticacion
{
    public string Email { get; }
    public string Clave { get;}
    
    public DatosAutenticacion(string email, string clave)
    {
        Email = email;
        Clave = clave;
    }
}