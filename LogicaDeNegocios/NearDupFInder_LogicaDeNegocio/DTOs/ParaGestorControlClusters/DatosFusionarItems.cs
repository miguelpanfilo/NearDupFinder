namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters
{
    public class DatosFusionarItems
    {
        public int IdCluster { get; }
        public int IdCatalogo { get; }

        public DatosFusionarItems(int idCatalogo, int idCluster)
        {
            IdCluster = idCluster;
            IdCatalogo = idCatalogo;
        }
    }
}