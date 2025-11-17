namespace NearDupFinder_Dominio.Excepciones;

public class ExcepcionDuplicado : Exception
{
    public ExcepcionDuplicado(string message) : base(message)
    {
        
    }
}