namespace NearDupFinder_Dominio;

public class Catalogo
{
    public const int TituloMaxLength = 120;
    public const int DescripcionMaxLength = 400;
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
        string d = (descripcion ?? "").Trim();

        if (d.Length > DescripcionMaxLength)
        {
            throw new ArgumentException($"La descripcion debe tener entre 1 y {DescripcionMaxLength} caracteres");
        }
        
        Descripcion = d;
    }
    
    private void EstablecerTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El titulo es obligatorio");
        }else if (titulo.Length > TituloMaxLength)
        {
            throw new ArgumentException("El titulo debe tener entre 1 y 120 caracteres");
        }
        
        Titulo = titulo.Trim();
    }

}