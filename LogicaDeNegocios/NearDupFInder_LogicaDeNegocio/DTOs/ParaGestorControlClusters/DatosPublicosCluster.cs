using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;

public class DatosPublicosCluster
{
    public int Id { get; }
    public IReadOnlyCollection<DatosItemListaItems> PertenecientesCluster { get; }
    public DatosItemListaItems? Canonico { get; }

    private DatosPublicosCluster(
        int id,
        IReadOnlyCollection<DatosItemListaItems> pertenecientesCluster,
        DatosItemListaItems? canonico)
    {
        Id = id;
        PertenecientesCluster = pertenecientesCluster;
        Canonico = canonico;
    }

    public static DatosPublicosCluster FromEntity(Cluster cluster)
    {
        var itemsDtos = cluster.PertenecientesCluster
            .Select(DatosItemListaItems.FromEntity)
            .ToList()
            .AsReadOnly();

        DatosItemListaItems? canonicoDto = cluster.Canonico is not null
            ? DatosItemListaItems.FromEntity(cluster.Canonico)
            : null;

        return new DatosPublicosCluster(
            cluster.Id,
            itemsDtos,
            canonicoDto
        );
    }
    
}