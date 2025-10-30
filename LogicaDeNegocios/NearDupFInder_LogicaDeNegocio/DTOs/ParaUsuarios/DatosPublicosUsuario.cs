using NearDupFinder_Dominio.Clases;

namespace NearDupFInder_LogicaDeNegocio.DTOs.ParaUsuarios;

public class DatosPublicosUsuario(
    string nombre,
    string apellido,
    string email,
    DateTime fechaNacimiento,
    List<string> roles)
{
    public readonly string Nombre = nombre;
    public readonly string Apellido = apellido;
    public readonly string Email = email;
    public readonly DateTime FechaNacimiento = fechaNacimiento;
    public readonly IReadOnlyCollection<string> Roles = roles;

    public static DatosPublicosUsuario FromEntity(Usuario usuario)
    {
        List<string> roles = [];
        foreach (Rol rol in usuario.ObtenerRoles())
        {
            roles.Add(rol.ToString());
        }

        return new DatosPublicosUsuario(
            usuario.Nombre,
            usuario.Apellido,
            usuario.Email.ToString(),
            usuario.FechaNacimiento.ToDateTime(),
            roles);
    }
}