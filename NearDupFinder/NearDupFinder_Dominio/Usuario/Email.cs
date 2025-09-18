using System.ComponentModel.DataAnnotations;

namespace NearDupFinder_Dominio.Usuario;

public sealed class Email
{
    private readonly string _valor;
    
    private Email(string email) => _valor = email;

    public static Email Crear(string email)
    {
        if (email is null)
            throw new ArgumentNullException(nameof(email));
        
        if (!EsFormatoValido(email))
            throw new ArgumentException("El email no tiene un formato válido.", nameof(email));

        return new Email(email);
    }
    
    public override string ToString() => _valor;
    
    private static bool EsFormatoValido(string correo)
    {
        // Validación estándar de .NET
        var atributo = new EmailAddressAttribute();
        if (!atributo.IsValid(correo)) return false;

        // Ej.: "usuario@ejemplo.com" → dominio = "ejemplo.com".
        var indiceArroba = correo.IndexOf('@');
        var dominio = correo[(indiceArroba + 1)..];

        if (!dominio.Contains('.')) return false;   // “usuario@ejemplo” => inválido
        if (dominio.StartsWith('.') || dominio.EndsWith('.')) return false; // usuario@.com o usuario@ejemplo.  => inválidos
        if (correo.Contains("..")) return false; // Prohibimos puntos consecutivos

        return true;
    }
}