using System.Security.Cryptography;
using System.Text;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Clave
{
    private const int LargoMinimo = 8;

    private static int _siguienteId = 1;

    private int Id { get; set; }
    
    private string Hash { get; set; }

    public Clave()
    {
        Hash = String.Empty;
        Id = _siguienteId++;
    }

    private Clave(string hash)
    {
        Hash = hash;
    }
    
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
    
    private static string HashearMd5(string texto)
    {
        using var md5 = MD5.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(texto);
        byte[] hash = md5.ComputeHash(bytes);
        string hashString = Convert.ToHexString(hash);
        
        return hashString;
    }
    
    public static Clave Crear(string? clave)
    {
        if (Validar(clave))
        {
            var hash = HashearMd5(clave!);
            return new Clave(hash);
        }
 
        throw new ExcepcionDeUsuario("La contraseña no tiene el formato valido");        
    }
    
    public bool Verificar(string clave)
    {
        return string.Equals(HashearMd5(clave), Hash);
    }

    public string ObtenerHash()
    {
        return Hash;
    }
}