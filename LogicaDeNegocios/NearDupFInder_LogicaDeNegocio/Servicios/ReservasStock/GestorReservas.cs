using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFInder_LogicaDeNegocio.Servicios.ReservasStock;

public static class GestorReservas
{
    public static bool Aplicar(Cluster cluster, int cantidad)
    {
        if (!cluster.StockMinimoCanonico.HasValue ||
            !cluster.PrecioCanonico.HasValue)
        {
            throw new ExcepcionCluster(
                "El clúster no tiene configurados precio y/o umbral canónico."
            );
        }
        if (cantidad <= 0) throw new InvalidOperationException("Cantidad inválida.");
        if (cluster.StockActual < cantidad) throw new InvalidOperationException("Stock insuficiente.");

        int restante = cantidad;

        var itemsOrdenados = cluster.PertenecientesCluster
            .OrderBy(i => i.Stock)
            .ThenBy(i => i.Id)
            .ToList();

        foreach (var item in itemsOrdenados)
        {
            const int cantidadItemsVacio = 0;
            if (restante == cantidadItemsVacio) break;
            int tomar = Math.Min(item.Stock, restante);
            item.Stock -= tomar;
            restante   -= tomar;
        }

        return true;
    }
}