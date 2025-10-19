namespace NearDupFinder_Dominio.Clases;


public class Catalogo
{
    private const int TituloMaxLength = 120;
    private const int DescripcionMaxLength = 400;
    private const int CantidadMinimaParaQueClusterExista = 2;
    private int _nextClusterId = 1;
    private static int _nextId = 1;
    public int Id { get; }
    public string Titulo { get; private set; } = null!;
    public string Descripcion { get; private set; } = string.Empty;
    private readonly List<Item> _items = new();
    private readonly Dictionary<int, Cluster> _clusters = new();
    
    
    public Catalogo(string titulo)
    {
        Id = _nextId++;
        EstablecerTitulo(titulo);
    }
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
        if(item == null)
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
        if(item == null)
            throw new ArgumentNullException(nameof(item), "El parametro no puede ser Null");
        if (!_items.Contains(item))
            throw new InvalidOperationException("El item no se encuentra en el catálogo");
        QuitarItemDeCluster(item);
        _items.Remove(item);
    }
    public int CantidadItems()
    {
        return _items.Count;
    }
    
    public IEnumerable<Cluster> Clusters => _clusters.Values;
    
    public void ConfirmarClusters(Item itemParaCluster1, Item itemParaCluster2)
    {
        if (itemParaCluster1 == null) 
            throw new ArgumentNullException(nameof(itemParaCluster1), "El parametro no puede ser null");
        
        if (itemParaCluster2 == null) 
            throw new ArgumentNullException(nameof(itemParaCluster2), "El parametro no puede ser null");
        
        if (!_items.Contains(itemParaCluster1) || !_items.Contains(itemParaCluster2))
            throw new InvalidOperationException("Uno o ambos ítems no pertenecen al catalogo");
        
        if (_clusters.Values.Any(c => c.Contiene(itemParaCluster1) && c.Contiene(itemParaCluster2)))
            return;
        
        if (itemParaCluster1.Id == itemParaCluster2.Id)
            return;
        
        var clusterDe1 = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(itemParaCluster1));
        var clusterDe2 = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(itemParaCluster2));
        
        if (clusterDe1 != null && clusterDe2 == null)
        {
            clusterDe1.Agregar(itemParaCluster2);
            return;
        }

        if (clusterDe1 == null && clusterDe2 != null)
        {
            clusterDe2.Agregar(itemParaCluster1);
            return;
        }
        
        if (clusterDe1 != null && clusterDe2 != null && clusterDe1.Id != clusterDe2.Id)
        {
            foreach (var item in clusterDe2.PertenecientesCluster.ToList())
                clusterDe1.Agregar(item);

            _clusters.Remove(clusterDe2.Id);
            return;
        }
        
        var nuevoId = _nextClusterId++;
        
        _clusters[nuevoId] = new Cluster(nuevoId, new HashSet<Item> { itemParaCluster1, itemParaCluster2 });
    }
    
    public void QuitarItemDeCluster(Item itemParaQuitar)
    {
        if (itemParaQuitar == null) 
            throw new ArgumentNullException(nameof(itemParaQuitar), "El parámetro no puede ser null");
        
        if (!_items.Contains(itemParaQuitar)) 
            throw new InvalidOperationException("El item no pertenece al catalogo");

        var cluster = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(itemParaQuitar));
        
        if (cluster is null)
            return;
        
        cluster.Remover(itemParaQuitar);
        
        if (cluster.PertenecientesCluster.Count() < CantidadMinimaParaQueClusterExista) { _clusters.Remove(cluster.Id); }
    }
    
    public Cluster? ObtenerClusterDe(Item item)
    {
        return _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(item));
    }

    public Cluster? ObtenerClusterPorId(int id)
    {
        return _clusters.TryGetValue(id, out var cluster) ? cluster : null;
    }
}