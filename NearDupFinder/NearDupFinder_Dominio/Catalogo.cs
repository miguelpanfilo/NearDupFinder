namespace NearDupFinder_Dominio;

public class Catalogo
{
    public string Titulo { get; private set; }
    public string Descripcion { get; private set; } = "";
    

    public Catalogo(string titulo)
    {
        EstablecerTitulo(titulo);
    }
    public void CambiarTitulo(string titulo)
    {
        EstablecerTitulo(titulo);
    }

    public void CambiarDescripcion(string? descripcion)
    {
        this.Descripcion = (descripcion ?? "").Trim();
    }
    
    private void EstablecerTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El titulo es obligatorio");

        Titulo = titulo.Trim();
    }

}