namespace NearDupFinder_Dominio.Clases;

public class Cluster
{
    public int Id { get; }
    private readonly HashSet<Item> _pertenecientesCluster;

    public IEnumerable<Item> PertenecientesCluster => _pertenecientesCluster;

    public  Cluster(int id, HashSet<Item> pertenecientesCluster)
    {
        Id = id;
        _pertenecientesCluster = pertenecientesCluster;
    }
}