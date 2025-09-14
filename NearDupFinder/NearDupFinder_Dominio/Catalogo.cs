namespace NearDupFinder_Dominio;

public class Catalogo
{
    public String Titulo { get; private set; }

    public Catalogo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El titulo es obligatorio");
        }
        
        this.Titulo = titulo;
    }
}