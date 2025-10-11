namespace NearDupFinder_Dominio.Clases;


public class Catalogo
{
    private const int TituloMaxLength = 120;
    private const int DescripcionMaxLength = 400;
    public string Titulo { get; private set; }
    public string Descripcion { get; private set; } = "";
    private readonly List<Item> _items = new();
    
    
    private readonly Dictionary<int, Cluster> _clusters = new();
    private int _nextClusterId = 1;
    private int _cantidadMinimaParaQueClusterExista = 2;
    public IEnumerable<Cluster> Clusters => _clusters.Values;
    

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
        }
        if (titulo.Length > TituloMaxLength)
        {
            throw new ArgumentException($"El titulo debe tener entre 1 y {TituloMaxLength} caracteres");
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
        QuitarItemDeCluster(item);
        _items.Remove(item);
    }
    public int CantidadItems()
    {
        return _items.Count;
    }
    
    /* Espacio Cluster*/
    
    public void ConfirmarClusters(Item a, Item b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a), "El parametro no puede ser null");
        if (b == null) throw new ArgumentNullException(nameof(b), "El parametro no puede ser null");
        if (!_items.Contains(a) || !_items.Contains(b))
            throw new InvalidOperationException("Uno o ambos ítems no pertenecen al catalogo");
        if (_clusters.Values.Any(c => c.Contiene(a) && c.Contiene(b)))
            return;
        
        if (a.Id == b.Id)
            return;
        
        var clusterDeA = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(a));
        var clusterDeB = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(b));
        
        if (clusterDeA != null && clusterDeB == null)
        {
            clusterDeA.Agregar(b);
            return;
        }

        if (clusterDeA == null && clusterDeB != null)
        {
            clusterDeB.Agregar(a);
            return;
        }
        
        if (clusterDeA != null && clusterDeB != null && clusterDeA.Id != clusterDeB.Id)
        {
            foreach (var item in clusterDeB.PertenecientesCluster.ToList())
                clusterDeA.Agregar(item);

            _clusters.Remove(clusterDeB.Id);
            return;
        }
        
        var nuevoId = _nextClusterId++;
        
        _clusters[nuevoId] = new Cluster(nuevoId, new HashSet<Item> { a, b });
    }
    
    public void QuitarItemDeCluster(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item), "El parámetro no puede ser null");
        if (!_items.Contains(item)) throw new InvalidOperationException("El item no pertenece al catalogo");
        
        var cluster = _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(item));
        if (cluster == null)
            return;
        
        cluster.Remover(item);
        
        if (cluster.PertenecientesCluster.Count() < _cantidadMinimaParaQueClusterExista) { _clusters.Remove(cluster.Id); }
    }
    
    public Cluster? ObtenerClusterDe(Item item)
    {
        return _clusters.Values.FirstOrDefault(c => c.PertenecientesCluster.Contains(item));
    }
}