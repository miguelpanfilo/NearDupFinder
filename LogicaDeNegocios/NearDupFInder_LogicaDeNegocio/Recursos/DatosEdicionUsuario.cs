using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Recursos;

public class DatosEdicionUsuario
{
    public string EmailActual { get; }
    public string? NuevoNombre { get; }
    public string? NuevoApellido { get; }
    public int Anio { get; }
    public int Mes { get; }
    public int Dia { get; }
    public string? NuevaClave { get; }
    public IReadOnlyCollection<Rol>? NuevosRoles { get; }

    public DatosEdicionUsuario(
        string email,
        string? nuevoNombre,
        string? nuevoApellido,
        int anio,
        int mes,
        int dia,
        string? nuevaClave,
        List<Rol>? nuevosRoles)
    {
        EmailActual = email.Trim() ?? "";
        NuevoNombre = nuevoNombre?.Trim();
        NuevoApellido = nuevoApellido?.Trim();
        Anio = anio;
        Mes = mes;
        Dia = dia;
        NuevaClave = nuevaClave;
        NuevosRoles = nuevosRoles;
    }
}