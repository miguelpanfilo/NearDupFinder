namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorUsuario;

public class DatosCambioClave(string? email, string? claveActual, string? claveNueva)
{
    public string Email { get; } = (email ?? "").Trim();
    public string ClaveActual { get; } = claveActual ?? "";
    public string ClaveNueva { get; } = claveNueva  ?? "";
}