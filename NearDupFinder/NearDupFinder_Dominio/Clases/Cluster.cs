namespace NearDupFinder_Dominio.Clases;

public class Cluster
{
    public int Id { get; }
    private readonly HashSet<Item> _pertenecientesCluster;
    public Item Canonico { get; private set; }
    public  IEnumerable<Item> PertenecientesCluster => _pertenecientesCluster;

    public  Cluster(int id, HashSet<Item> pertenecientesCluster)
    {
        Id = id;
        _pertenecientesCluster = pertenecientesCluster;
        ActualizarCanonico();
    }
    
    public void Agregar(Item item) => _pertenecientesCluster.Add(item);
    
    public void Remover(Item item) => _pertenecientesCluster.Remove(item);
    public bool Contiene(Item item) => _pertenecientesCluster.Contains(item);

    private void ActualizarCanonico()
    {
        Canonico = _pertenecientesCluster
            .OrderByDescending(i => i.Descripcion.Length)
            .ThenByDescending(i => i.Titulo.Length)
            .ThenBy(i => i.Id)
            .First();
        ActualizarBanderaCanonica();
    }

    private void ActualizarBanderaCanonica()
    {
        foreach (var it in _pertenecientesCluster)
        {
            if (it == Canonico)
                it.EsCanonico = true;
            else
                it.EsCanonico = false;
        }
    }
    
}