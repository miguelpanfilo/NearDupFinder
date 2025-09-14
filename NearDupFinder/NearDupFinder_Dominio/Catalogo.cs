namespace NearDupFinder_Dominio;

public class Catalogo
{
    public String Titulo { get; private set; }

    public Catalogo(string titulo)
    {
        this.Titulo = titulo;
    }
}