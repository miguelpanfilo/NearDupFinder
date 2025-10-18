namespace NearDupFinder_LogicaDeNegocio.Recursos;

public class DatosCambioClave
{
    public string Email { get; }
    public string ClaveActual { get; }
    public string ClaveNueva { get; }

    public DatosCambioClave(string? email, string? claveActual, string? claveNueva)
    {
        Email = (email ?? "").Trim();
        ClaveActual = claveActual ?? "";
        ClaveNueva  = claveNueva  ?? "";
    }
}