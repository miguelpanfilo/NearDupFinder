using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;

public class DatosEdicionUsuario(
    string email,
    string nuevoNombre,
    string nuevoApellido,
    int anio,
    int mes,
    int dia,
    string nuevaClave,
    List<string> roles)
{
    public readonly string EmailActual = email;
    public readonly string NuevoNombre = nuevoNombre;
    public readonly string NuevoApellido = nuevoApellido;
    public readonly int Anio = anio;
    public readonly int Mes = mes;
    public readonly int Dia = dia;
    public readonly string NuevaClave = nuevaClave;
    public readonly IReadOnlyCollection<Rol> NuevosRoles = ConvertirListaStringARoles(roles);


    private static IReadOnlyCollection<Rol> ConvertirListaStringARoles(List<string> roles)
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