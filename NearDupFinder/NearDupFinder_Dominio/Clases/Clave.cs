using System.Security.Cryptography;
using System.Text;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Clave
{
    private const int LargoMinimo = 8;

    private readonly string _hash;

    public Clave()
    {
        _hash = String.Empty;
    }

    private Clave(string hash)
    {
        _hash = hash;
    }

    /* Valida que se cumplan las siguientes condiciones:
     Longitud mínima: La contraseña debe tener al menos 8 caracteres.
     Debe incluir al menos una letra mayúscula (A-Z).
     Debe incluir al menos una letra minúscula (a-z).
     Debe incluir al menos un número (0-9).
     Debe incluir al menos un carácter especial (como @, #, $, etc.).
     */
    public static bool Validar(string? clave)
    {
        if (clave is null
            || clave.Length < LargoMinimo
            || !clave.Any(char.IsUpper)
            || !clave.Any(char.IsLower)
            || !clave.Any(char.IsDigit)
            || !clave.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
           )
            return false;
        
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
        //  Convert ToHexString devuelve en mayúsculas (p. ej. "E10ADC...").
        string hashString = Convert.ToHexString(hash);

        // Devolvemos la representación en hex, que es lo que
        // almacenaremos/comparemos para no guardar el texto plano.
        return hashString;
    }
    
    public static Clave Crear(string? clave)
    {
        if (Validar(clave))
        {
            var hash = HashearMd5(clave!);
            return new Clave(hash);
        }
 
        throw new UsuarioException("La contraseña no tiene el formato valido");        
    }
    
    public bool Verificar(string clave)
    {
        return string.Equals(HashearMd5(clave), _hash);
    }

    public string ObtenerHash()
    {
        return _hash;
    }
}