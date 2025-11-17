using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters
{
    public class DatosPublicosCluster
    {
        public int Id { get; }
        public IReadOnlyCollection<DatosItemListaItems> PertenecientesCluster { get; }
        public DatosItemListaItems? Canonico { get; }
        public string? ImagenCanonicaBase64 { get; }
        public int? StockMinimoCanonico { get; }
        public int? PrecioCanonico { get; }
        public int StockTotal { get; }

        private DatosPublicosCluster(
            int id,
            IReadOnlyCollection<DatosItemListaItems> pertenecientesCluster,
            DatosItemListaItems? canonico,
            string? imagenCanonicaBase64,
            int? stockMinimoCanonico,
            int? precioCanonico,
            int stockTotal)
        {
            Id = id;
            PertenecientesCluster = pertenecientesCluster;
            Canonico = canonico;
            ImagenCanonicaBase64 = imagenCanonicaBase64;
            StockMinimoCanonico = stockMinimoCanonico;
            PrecioCanonico = precioCanonico;
            StockTotal = stockTotal;
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
                canonicoDto,
                cluster.ImagenCanonicaBase64,
                cluster.StockMinimoCanonico,
                cluster.PrecioCanonico,
                cluster.StockActual
            );
        }
    }
}