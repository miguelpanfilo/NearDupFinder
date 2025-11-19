using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaLogin;

public class DatosIdentificacion(string nombre, string email, int id, IReadOnlyCollection<Rol> roles)
{
    public string Nombre { get; } = nombre;
    public string Email { get; } = email;
    public int Id { get; } = id;

    public readonly List<string> Roles = ConvertirListaRolesAString(roles);
    
    private static List<string> ConvertirListaRolesAString(IReadOnlyCollection<Rol> roles)
    {
        return roles
            .Select(r => r.ToString())
            .Distinct()
            .ToList();
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