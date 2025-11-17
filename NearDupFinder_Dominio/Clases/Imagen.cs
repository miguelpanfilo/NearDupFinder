using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public class Imagen
{
    private const int MaxBytes = 1 * 1024 * 1024;

    public string Base64 { get; }

    private Imagen(string base64)
    {
        Base64 = base64;
    }

    public static Imagen CrearDesdeBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            throw new ExcepcionItem("La imagen es obligatoria.");

        byte[] bytes;

        try
        {
            bytes = Convert.FromBase64String(base64);
        }
        catch (FormatException)
        {
            throw new ExcepcionItem("La imagen no tiene un formato Base64 válido.");
        }

        if (bytes.Length > MaxBytes)
            throw new ExcepcionItem("La imagen no puede superar 1 MB.");

        return new Imagen(base64);
    }
}