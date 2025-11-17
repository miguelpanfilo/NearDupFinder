namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters
{
    public class DatosConfigurarCanonicoCluster
    {
        public int IdCatalogo { get; }
        public int IdCluster { get; }
        public int? IdItemImagenSeleccionada { get; }
        public int? StockMinimo { get; }
        public int? PrecioSeleccionado { get; }

        public DatosConfigurarCanonicoCluster(
            int idCatalogo,
            int idCluster,
            int? idItemImagenSeleccionada,
            int? stockMinimo,
            int? precioSeleccionado)
        {
            IdCatalogo = idCatalogo;
            IdCluster = idCluster;
            IdItemImagenSeleccionada = idItemImagenSeleccionada;
            StockMinimo = stockMinimo;
            PrecioSeleccionado = precioSeleccionado;
        }
    }
}