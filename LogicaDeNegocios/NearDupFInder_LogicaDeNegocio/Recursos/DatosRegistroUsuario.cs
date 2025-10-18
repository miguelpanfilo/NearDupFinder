using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Recursos;

public class DatosRegistroUsuario
{
    public string? Nombre { get; }
    public string? Apellido { get; }
    public string? Email { get; }
    public int Anio { get; }
    public int Mes { get; } 
    public int Dia { get; }
    public string? Clave { get; }
    public IReadOnlyCollection<Rol>? Roles;

    public  DatosRegistroUsuario(string? nombre, string? apellido, string? email, int anio, int mes, int dia, string? clave, List<Rol> roles)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Anio = anio;
        Mes = mes;
        Dia = dia;
        Clave = clave;
        Roles = roles;
    }
}