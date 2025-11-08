namespace NearDupFinder_Dominio.Clases;

public class Catalogo
{
    private const int TituloMaxLength = 120;
    private const int DescripcionMaxLength = 400;
    public readonly int CantidadMinimaParaQueClusterExista = 2;

    private int _nextClusterId = 1;
    private static int _nextId = 1;
    public int Id { get; private set;}
    public string Titulo { get; private set; } = null!;
    public string Descripcion { get; private set; } = string.Empty;
    private readonly List<Item> _items = new();
    private readonly Dictionary<int, Cluster> _clusters = new();


    public Catalogo(string titulo)
    {
        Id = _nextId;
        EstablecerTitulo(titulo);
        _nextId++;
    }
    
    protected Catalogo() { }

    public void CambiarTitulo(string titulo)
    {
        EstablecerTitulo(titulo);
    }

    public void CambiarDescripcion(string? descripcion)
    {
        string descripcionEntrante = (descripcion ?? "").Trim();

        if (descripcionEntrante.Length > DescripcionMaxLength)
        {
            throw new ArgumentException($"La descripcion debe tener entre 1 y {DescripcionMaxLength} caracteres");
        }

        Descripcion = descripcionEntrante;
    }

    private void EstablecerTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El titulo es obligatorio");
        }

        if (titulo.Length > TituloMaxLength)
        {
            throw new ArgumentException($"El titulo debe tener entre 1 y {TituloMaxLength} caracteres");
        }

        Titulo = titulo.Trim();
    }

    public bool Equals(Catalogo? other)
    {
        if (other is null) return false;
        return Id == other.Id ||
               string.Equals(Titulo, other.Titulo, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
        => obj is Catalogo c && Equals(c);

    public override int GetHashCode() => Id.GetHashCode();
    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();

    public void AgregarItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "El parametro no puede ser Null");
        if (_items.Contains(item))
            throw new InvalidOperationException("El item ya se encuentra en el catálogo");
        _items.Add(item);
    }

    public Item? ObtenerItemPorId(int id)
    {
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public void EliminarItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "El parametro no puede ser Null");
        if (!_items.Contains(item))
            throw new InvalidOperationException("El item no se encuentra en el catálogo");
        _items.Remove(item);
    }

    public int CantidadItems()
    {
        return _items.Count;
    }
    
    public IReadOnlyCollection<Cluster> Clusters => _clusters.Values.ToList().AsReadOnly();

    public void CrearCluster(HashSet<Item> integrantes)
    {
        var nuevoId = _nextClusterId++;
        var c = new Cluster(nuevoId, integrantes);
        _clusters[nuevoId] = c;
    }

    public void AgregarItemACluster(Cluster c, Item item) => c.Agregar(item);

    public void RemoverItemDeCluster(Cluster c, Item item) => c.Remover(item);

    public void EliminarCluster(Cluster c) => _clusters.Remove(c.Id);
    
    public Cluster? ObtenerClusterDe(Item item)
    {
        return _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(item));
    }

    public Cluster? ObtenerClusterPorId(int id)
    {
        return _clusters.TryGetValue(id, out var cluster) ? cluster : null;
    }
    public static void ResetearContadorIdDesde(int nuevoValor)
    {
        if (nuevoValor > 0)
            _nextId = nuevoValor;
    }

}