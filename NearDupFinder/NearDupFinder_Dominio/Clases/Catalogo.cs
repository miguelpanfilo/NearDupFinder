namespace NearDupFinder_Dominio.Clases;


public class Catalogo
{
    private const int tituloMaxLength = 120;
    private const int descripcionMaxLength = 400;
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

        if (d.Length > descripcionMaxLength)
        {
            throw new ArgumentException($"La descripcion debe tener entre 1 y {descripcionMaxLength} caracteres");
        }
        
        Descripcion = d;
    }
    
    private void EstablecerTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El titulo es obligatorio");
        }else if (titulo.Length > tituloMaxLength)
        {
            throw new ArgumentException($"El titulo debe tener entre 1 y {tituloMaxLength} caracteres");
        }
        
        Titulo = titulo.Trim();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Catalogo otro) return false;

        return Titulo.Equals(otro.Titulo, StringComparison.OrdinalIgnoreCase);
    }
    
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Titulo);
    }
}