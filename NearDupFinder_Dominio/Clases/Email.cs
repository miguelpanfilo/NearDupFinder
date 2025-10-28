using System.ComponentModel.DataAnnotations;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Email
{
    private Email() { }
    
    public string Valor { get; private set; } = null!;

    public Email(string email)
    {    
        Valor = email;
    }

    public static Email Crear(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ExcepcionDeUsuario("El email es requerido.");
        
        var emailNormalizado = email.Trim().ToLowerInvariant();
        
        if (!EsFormatoValido(emailNormalizado))
            throw new ExcepcionDeUsuario("El email no tiene un formato válido.");

        return new Email(emailNormalizado);
    }

    public override string ToString()
    {
        return Valor;
    }
    
    private static bool EsFormatoValido(string correo)
    {
        var atributo = new EmailAddressAttribute();
        if (!atributo.IsValid(correo)) 
            return false;

        var indiceArroba = correo.IndexOf('@');
        var dominio = correo[(indiceArroba + 1)..];

        if (!dominio.Contains('.')) 
            return false;
        if (dominio.StartsWith('.') || dominio.EndsWith('.')) 
            return false;
        if (correo.Contains("..")) 
            return false;

        return true;
    }
    
    public bool Igual(Email otroEmail)
    {
        return string.Equals(ToString(), otroEmail.ToString());
    }
}