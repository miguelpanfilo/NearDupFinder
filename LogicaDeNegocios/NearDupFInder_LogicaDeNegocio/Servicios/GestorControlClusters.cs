using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorDuplicados;
using NearDupFInder_LogicaDeNegocio.Servicios.Duplicados;


namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorControlClusters
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly ControladorDuplicados _controladorDuplicados;
    private readonly GestorAuditoria _gestorAuditoria;

    public GestorControlClusters(
        GestorCatalogos gestorCatalogos,
        ControladorDuplicados controladorDuplicados,
        GestorAuditoria gestorAuditoria)
    {
        _gestorCatalogos = gestorCatalogos;
        _controladorDuplicados = controladorDuplicados;
        _gestorAuditoria = gestorAuditoria;
    }

    public void ConfirmarParDuplicado(DatosDuplicados duplicadoConfirmado)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(duplicadoConfirmado.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={duplicadoConfirmado.IdCatalogo}).");

        var itemA = catalogo.ObtenerItemPorId(duplicadoConfirmado.IdItemAComparar)
                    ?? throw new ExcepcionItem($"Ítem no encontrado (Id={duplicadoConfirmado.IdItemAComparar}).");

        var itemB = catalogo.ObtenerItemPorId(duplicadoConfirmado.IdItemPosibleDuplicado)
                    ?? throw new ExcepcionItem(
                        $"Ítem no encontrado (Id={duplicadoConfirmado.IdItemPosibleDuplicado}).");

        catalogo.ConfirmarClusters(itemA, itemB);

        _controladorDuplicados.DescartarParDuplicado(duplicadoConfirmado);

        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.ConfirmarDuplicado,
            $"Confirmado duplicado en catálogo {catalogo.Id}: '{itemA.Titulo}' (Id={itemA.Id}) ↔ '{itemB.Titulo}' (Id={itemB.Id})."
        );
    }

    public void BorrarItemDelCluster(DatosRemoverItemCluster datosRemoverItemCluster)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datosRemoverItemCluster.IdCatalogo);

        if (catalogo is null) return;

        var itemRemover = ObtenerItemCatalogo(catalogo, datosRemoverItemCluster.IdItem);

        if (itemRemover is null) return;

        var cluster = catalogo.ObtenerClusterDe(itemRemover);

        if (cluster is null) return;

        var canonico = cluster.Canonico;

        if (canonico is not null && canonico.Equals(itemRemover))
            AsignarNuloCanonico(cluster);

        RemoverItemDelCluster(catalogo, itemRemover);
    }

    private Item? ObtenerItemCatalogo(Catalogo catalogo, int id)
    {
        return catalogo.Items.First(i => i.Id == id);
    }

    private void RemoverItemDelCluster(Catalogo catalogo, Item itemRemover)
    {
        catalogo.QuitarItemDeCluster(itemRemover);
    }

    private void AsignarNuloCanonico(Cluster? cluster)
    {
        if (cluster is not null)
            cluster.Canonico = null;
    }

    public void FusionarItemsEnElCluster(DatosFusionarItems datosFusionarItem)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datosFusionarItem.IdCatalogo);

        if (catalogo is null) return;

        var clusterAFusionar = catalogo.ObtenerClusterPorId(datosFusionarItem.IdCluster);

        if (clusterAFusionar is null) return;

        bool fusionado = clusterAFusionar.FusionarCanonico();

        if (fusionado)
        {
            if (clusterAFusionar.Canonico != null)
                _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.FusionarCluster,
                    $"Se fusionó el canónico del cluster '{clusterAFusionar.Canonico.Titulo}' con {clusterAFusionar.PertenecientesCluster.Count()} ítems.");
        }
    }

    public bool ItemEstaEnCluster(int idCatalogo, int idItem)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

        var item = catalogo.ObtenerItemPorId(idItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={idItem}).");

        return catalogo.ObtenerClusterDe(item) is not null;
    }
}