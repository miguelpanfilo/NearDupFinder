namespace NearDupFinder_Dominio.Clases;


public class Catalogo
{
    private const int tituloMaxLength = 120;
    private const int descripcionMaxLength = 400;
    public string Titulo { get; private set; }
    public string Descripcion { get; private set; } = "";
    private readonly List<Item> _items = new();
    
    

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
        }
        if (titulo.Length > tituloMaxLength)
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
    /* Lista Items*/
    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();


    public void AgregarItem(Item item)
    {
        if(item == null)
            throw new ArgumentNullException(nameof(item), "El parametro no puede ser Null");
        if (_items.Contains(item))
            throw new InvalidOperationException("El item ya se encuentra en el catálogo");
        _items.Add(item);
    }

    public void EliminarItem(Item item)
    {
        if(item == null)
            throw new ArgumentNullException(nameof(item), "El parametro no puede ser Null");
        if (!_items.Contains(item))
            throw new InvalidOperationException("El item no se encuentra en el catálogo");
        _items.Remove(item);
    }
    public int CantidadItems()
    {
        return _items.Count;
    }
    
}