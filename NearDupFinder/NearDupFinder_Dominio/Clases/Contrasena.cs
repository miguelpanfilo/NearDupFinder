using System.Security.Cryptography;
using System.Text;

namespace NearDupFinder_Dominio.Clases;

public sealed class Contrasena
{
    private const int LargoMinimo = 8;

    private readonly string _hash;
    
    private readonly string _contrasenaDefault = "Encr1pt@do";

    public Contrasena() => _hash = HashearMd5(_contrasenaDefault);

    private Contrasena(string hash) => _hash = hash;
    
    /* Valida que se cumplan las siguientes condiciones:
     Longitud mínima: La contraseña debe tener al menos 8 caracteres.
     Debe incluir al menos una letra mayúscula (A-Z).
     Debe incluir al menos una letra minúscula (a-z).
     Debe incluir al menos un número (0-9).
     Debe incluir al menos un carácter especial (como @, #, $, etc.).
     */
    public static bool Validar(string? contrasena)
    {
        if (contrasena is null
            || contrasena.Length < LargoMinimo
            || !contrasena.Any(char.IsUpper)
            || !contrasena.Any(char.IsLower)
            || !contrasena.Any(char.IsDigit)
            || !contrasena.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
           )
        {
            return false;
        }
        return true;
    }
    /*
     MD5 es una función hash que transforma cualquier texto en una “huella” fija de 128 bits (32 caracteres hex).
     No cifra (no usa clave ni es reversible): mismo input ⇒ mismo hash.
     */
    private static string HashearMd5(string texto)
    {
        // Creamos una instancia del algoritmo MD5 provista por .NET.
        using var md5 = MD5.Create();

        // Convertimos el texto a bytes usando UTF-8.
        //  El hash trabaja sobre bytes, no sobre strings.
        byte[] bytes = Encoding.UTF8.GetBytes(texto);

        // Calculamos el hash MD5 del arreglo de bytes.
        // MD5 produce 128 bits = 16 bytes de salida.
        byte[] hash = md5.ComputeHash(bytes);

        // Convertimos esos 16 bytes a una cadena hexadecimal de 32 caracteres.
        //  Convert.ToHexString devuelve en mayúsculas (p. ej. "E10ADC...").
        string hashString = Convert.ToHexString(hash);

        // Devolvemos la representación en hex, que es lo que
        // almacenaremos/comparemos para no guardar el texto plano.
        return hashString;
    }
    
    public static Contrasena Crear(string contrasena)
    {
        if (Validar(contrasena))
        {
            var hash = HashearMd5(contrasena);
            return new Contrasena(hash);
        }
        else
        {
            throw new ArgumentException("La contraseña no tiene el formato valido");        
        }
    }
    
    public bool Verificar(string? contrasena)
    {
        if (contrasena is null)
            return false;
        
        return string.Equals(HashearMd5(contrasena), _hash, StringComparison.OrdinalIgnoreCase);
    }

    public string ObtenerHash()
    {
        return _hash;
    }
}