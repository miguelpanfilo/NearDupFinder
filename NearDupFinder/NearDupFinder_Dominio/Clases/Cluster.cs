namespace NearDupFinder_Dominio.Clases;

public class Cluster
{
    public int Id { get; }
    private readonly HashSet<Item> _pertenecientesCluster;
    public Item? Canonico { get; set; }
    public  IEnumerable<Item> PertenecientesCluster => _pertenecientesCluster;

    public  Cluster(int id, HashSet<Item> pertenecientesCluster)
    {
        Id = id;
        _pertenecientesCluster = pertenecientesCluster;
    }
    
    public void Agregar(Item item)
    {
        _pertenecientesCluster.Add(item);
    }
    
    public void Remover(Item item)
    {
        _pertenecientesCluster.Remove(item);
    }
    public bool Contiene(Item item) => _pertenecientesCluster.Contains(item);
    
    public void FuncionarCanonico()
    {
        if (_pertenecientesCluster.Count == 0)
        {
            Canonico = null!;
            return;
        }
        Canonico = _pertenecientesCluster
            .OrderByDescending(i => i.Descripcion.Length)
            .ThenByDescending(i => i.Titulo.Length)
            .ThenBy(i => i.Id)
            .First();
        FusionarCampos(Canonico, _pertenecientesCluster);
    }
    
    private void FusionarCampos(Item canonico, IEnumerable<Item> miembros)
    {
        canonico.Marca     = ElegirMejorCampo(canonico.Marca,     miembros.Select(m => m.Marca));
        canonico.Modelo    = ElegirMejorCampo(canonico.Modelo,    miembros.Select(m => m.Modelo));
        canonico.Categoria = ElegirMejorCampo(canonico.Categoria, miembros.Select(m => m.Categoria));
    }

    private static string AseguraLargo(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

    private static string ElegirMejorCampo(string actualCanonico, IEnumerable<string?> candidatos)
    {
        if (!string.IsNullOrWhiteSpace(actualCanonico)) return actualCanonico;

        var mejor = candidatos
            .Select(AseguraLargo)
            .Where(v => v.Length > 0)
            .OrderByDescending(v => v.Length)
            .ThenBy(v => v, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
        
        return mejor ?? actualCanonico;
    }
    
}