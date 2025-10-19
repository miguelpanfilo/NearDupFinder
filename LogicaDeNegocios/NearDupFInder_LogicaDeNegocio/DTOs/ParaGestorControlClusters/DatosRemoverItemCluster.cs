namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;

public class DatosRemoverItemCluster
{
    public int IdItem { get;}
    public int IdCatalogo { get;}

    public DatosRemoverItemCluster(int idItem, int idCatalogo)
    {
        IdItem = idItem;
        IdCatalogo = idCatalogo;
    }
}