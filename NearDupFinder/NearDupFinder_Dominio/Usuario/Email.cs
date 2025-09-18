namespace NearDupFinder_Dominio.Usuario;

public sealed class Email
{
    private readonly string _valor;
    
    private Email(string email) => _valor = email;

    public static Email Crear(string email)
    {
        return new Email(email);
    }
    
    public override string ToString() => _valor;
}