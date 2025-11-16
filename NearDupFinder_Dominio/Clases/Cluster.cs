using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public class Cluster
{
    public int Id { get; private set; }
    private readonly HashSet<Item> _pertenecientesCluster;
    public IReadOnlyCollection<Item> PertenecientesCluster => _pertenecientesCluster;
    public Item? Canonico { get; set; }
    public string? ImagenCanonicaBase64 { get; set; }
    public int? StockMinimoCanonico { get; set; }
    public int? PrecioCanonico { get; set; }

    public Cluster(int id, HashSet<Item> pertenecientesCluster)
    {
        Id = id;
        _pertenecientesCluster = pertenecientesCluster;
    }
    
    protected Cluster()
    {
        _pertenecientesCluster = new HashSet<Item>();
    }
    
    public void Agregar(Item item)
    {
        _pertenecientesCluster.Add(item);
    }

    public void Remover(Item item)
    {
        _pertenecientesCluster.Remove(item);
    }
    public int StockActual => _pertenecientesCluster.Sum(item => item.Stock);
    public bool Contiene(Item item) => _pertenecientesCluster.Contains(item);
    
    public void ConfigurarCanonico(string? imagenBase64, int? stockMinimo, int? precio)
    {
        if (stockMinimo is < 0)
            throw new ExcepcionItem("El stock mínimo no puede ser negativo.");

        if (precio is < 0)
            throw new ExcepcionItem("El precio no puede ser negativo.");

        if (!string.IsNullOrWhiteSpace(imagenBase64))
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(imagenBase64);

                const int MaxBytes = 1 * 1024 * 1024;
                if (bytes.Length > MaxBytes)
                    throw new ExcepcionItem("La imagen del canónico no puede superar 1 MB.");
            }
            catch (FormatException)
            {
                throw new ExcepcionItem("La imagen del canónico no tiene un formato Base64 válido.");
            }
        }

        ImagenCanonicaBase64 = imagenBase64;
        StockMinimoCanonico = stockMinimo;
        PrecioCanonico = precio;
    }

    public bool FusionarCanonico()
    {
        const int cantidadVaciaEnCluster = 0;
        if (_pertenecientesCluster.Count == cantidadVaciaEnCluster)
        {
            Canonico = null;
            return false;
        }

        var nuevoCanonico = _pertenecientesCluster
            .OrderByDescending(i => i.Descripcion!.Length)
            .ThenByDescending(i => i.Titulo!.Length)
            .ThenBy(i => i.Id)
            .First();

        bool cambio = Canonico == null || Canonico.Id != nuevoCanonico.Id;
        Canonico = nuevoCanonico;

        FusionarCampos(Canonico, _pertenecientesCluster);

        return cambio;
    }

    private void FusionarCampos(Item canonico, IReadOnlyCollection<Item> miembros)
    {
        canonico.Marca = ElegirMejorCampo(canonico.Marca, miembros.Select(m => m.Marca));
        canonico.Modelo = ElegirMejorCampo(canonico.Modelo, miembros.Select(m => m.Modelo));
        canonico.Categoria = ElegirMejorCampo(canonico.Categoria, miembros.Select(m => m.Categoria));
    }

    private static string AseguraLargo(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

    private static string? ElegirMejorCampo(string? actualCanonico, IEnumerable<string?> candidatos)
    {
        const int largoStringVacio = 0;
        if (!string.IsNullOrWhiteSpace(actualCanonico)) return actualCanonico;

        var mejor = candidatos
            .Select(AseguraLargo)
            .Where(v => v.Length > largoStringVacio)
            .OrderByDescending(v => v.Length)
            .ThenBy(v => v, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();

        return mejor ?? actualCanonico;
    }
}