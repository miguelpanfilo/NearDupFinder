using NearDupFinder_Dominio.Clases;

namespace NearDupFInder_LogicaDeNegocio.DTOs.ParaLogin;

public class DatosIdentificacion(string nombre, string email, int id, IReadOnlyCollection<Rol> roles)
{
    public string Nombre { get; } = nombre;
    public string Email { get; } = email;
    public int Id { get; } = id;

    public readonly List<string> Roles = ConvertirListaRolesAString(roles);
    
    private static List<string> ConvertirListaRolesAString(IReadOnlyCollection<Rol> roles)
    {
        List<string> listaDeRoles = [];
        foreach (Rol rol in roles)
        {
            if (rol == Rol.Administrador && !listaDeRoles.Contains("Administrador"))
                listaDeRoles.Add("Administrador");

            if (rol == Rol.Revisor && !listaDeRoles.Contains("Revisor"))
                listaDeRoles.Add("Revisor");
        }

        return listaDeRoles;
    }

    public static DatosIdentificacion FromEntity(Usuario usuario)
    {
        return new DatosIdentificacion(
            usuario.Nombre,
            usuario.Email.ToString(),
            usuario.Id,
            usuario.ObtenerRoles()
        );
    }
}