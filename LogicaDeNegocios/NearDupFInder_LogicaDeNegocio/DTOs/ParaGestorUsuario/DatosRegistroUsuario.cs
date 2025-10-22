using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;

public class DatosRegistroUsuario
{
    public string? Nombre { get; }
    public string? Apellido { get; }
    public string? Email { get; }
    public int Anio { get; }
    public int Mes { get; } 
    public int Dia { get; }
    public string? Clave { get; }
    
    public readonly IReadOnlyCollection<Rol> Roles;

    public  DatosRegistroUsuario(string? nombre, string? apellido, string? email, int anio, int mes, int dia, string? clave, List<string> roles)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Anio = anio;
        Mes = mes;
        Dia = dia;
        Clave = clave;
        Roles = ConvertirListaStringARoles(roles);
    }

    private List<Rol> ConvertirListaStringARoles(List<string> roles)
    {
        List<Rol> listaDeRoles = [];
        foreach (string rol in roles)
        {
            if (string.Equals(rol, "Administrador") && !listaDeRoles.Contains(Rol.Administrador))
            {
                listaDeRoles.Add(Rol.Administrador);
            }
            else if (string.Equals(rol, "Revisor") && !listaDeRoles.Contains(Rol.Revisor))
            {
                listaDeRoles.Add(Rol.Revisor);
            }
        }

        return listaDeRoles;
    }
}