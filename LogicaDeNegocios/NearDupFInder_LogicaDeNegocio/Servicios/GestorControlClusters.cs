using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;


namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorControlClusters
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly GestorAuditoria _gestorAuditoria;

    public GestorControlClusters(
        GestorCatalogos gestorCatalogos,
        GestorAuditoria gestorAuditoria)
    {
        _gestorCatalogos = gestorCatalogos;
        _gestorAuditoria = gestorAuditoria;
    }

    public bool ConfirmarCluster(DatosDuplicados datos)
    {
        var contexto = PrepararContexto(datos);
   
        if (SonMismoItem(contexto.ItemA, contexto.ItemB))
            return false;
   
        if (SonMismoCluster(contexto.ClusterA, contexto.ClusterB))
            return true;

        return EjecutarOperacionCluster(contexto);
    }

    private ContextoCluster PrepararContexto(DatosDuplicados datos)
    {
        var catalogo = ObtenerCatalogo(datos.IdCatalogo);
        var itemA = ObtenerItem(catalogo, datos.IdItemAComparar);
        var itemB = ObtenerItem(catalogo, datos.IdItemPosibleDuplicado);
        var clusterA = ObtenerClusters(catalogo, itemA);
        var clusterB = ObtenerClusters(catalogo, itemB);
   
        return new ContextoCluster(catalogo, itemA, itemB, clusterA, clusterB);
    }

    private bool SonMismoItem(Item itemA, Item itemB)
    {
        return itemA.Id == itemB.Id;
    }

    private bool EjecutarOperacionCluster(ContextoCluster contextoCluster)
    {
        return (contextoCluster.ClusterA, contextoCluster.ClusterB) switch
        {
            (not null, null) => AgregarItem(contextoCluster.Catalogo, contextoCluster.ClusterA, contextoCluster.ItemB, contextoCluster.ItemA),
            (null, not null) => AgregarItem(contextoCluster.Catalogo, contextoCluster.ClusterB, contextoCluster.ItemA, contextoCluster.ItemB),
            (not null, not null) => Fusionar(contextoCluster.Catalogo, contextoCluster.ClusterA, contextoCluster.ClusterB, contextoCluster.ItemA, contextoCluster.ItemB),
            (null, null) => Crear(contextoCluster.Catalogo, contextoCluster.ItemA, contextoCluster.ItemB)
        };
    }
    
    private Catalogo ObtenerCatalogo(int idCatalogo) =>
        _gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
        ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

    private Item ObtenerItem(Catalogo c, int id) =>
        c.ObtenerItemPorId(id) ?? throw new ExcepcionItem($"Ítem no encontrado (Id={id}).");

    private Cluster? ObtenerClusters(Catalogo c, Item a) =>
        c.ObtenerClusterDe(a);

    private static bool SonMismoCluster(Cluster? a, Cluster? b) =>
        a is not null && b is not null && a.Id == b.Id;

    private bool AgregarItem(Catalogo c, Cluster destino, Item itemAgregar, Item itemOtro)
    {
        c.AgregarItemACluster(destino, itemAgregar);
        LogConfirmacion(c, itemOtro, itemAgregar);
        return true;
    }

    private bool Fusionar(Catalogo c, Cluster a, Cluster b, Item itemA, Item itemB)
    {
        foreach (var it in b.PertenecientesCluster.ToList())
            c.AgregarItemACluster(a, it);

        c.EliminarCluster(b);
        LogConfirmacion(c, itemA, itemB);
        return true;
    }

    private bool Crear(Catalogo c, Item itemA, Item itemB)
    {
        c.CrearCluster(new HashSet<Item> { itemA, itemB });
        LogConfirmacion(c, itemA, itemB);
        return true;
    }

    private void LogConfirmacion(Catalogo c, Item a, Item b)
    {
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.ConfirmarDuplicado,
            $"Confirmado duplicado en catálogo {c.Id}: '{a.Titulo}' (Id={a.Id}) ↔ '{b.Titulo}' (Id={b.Id})."
        );
    }

    public void BorrarItemDelCluster(DatosRemoverItemCluster datosRemoverItemCluster)
    {
        var catalogo = ObtenerCatalogo(datosRemoverItemCluster.IdCatalogo);

        var item = ObtenerItem(catalogo, datosRemoverItemCluster.IdItem);

        var cluster = catalogo.ObtenerClusterDe(item);

        if (cluster is null) return;
        
        bool esCanonico = cluster.Canonico is not null && cluster.Canonico.Equals(item);
        
        catalogo.RemoverItemDeCluster(cluster, item);

        if (cluster.PertenecientesCluster.Count() < catalogo.CantidadMinimaParaQueClusterExista)
            catalogo.EliminarCluster(cluster);
        
        if (esCanonico)
            AsignarNuloCanonico(cluster);
    }

    private void AsignarNuloCanonico(Cluster? cluster)
    {
        if (cluster is not null)
            cluster.Canonico = null;
    }

    public void FusionarItemsEnElCluster(DatosFusionarItems datosFusionarItem)
    {
        var catalogo = ObtenerCatalogo(datosFusionarItem.IdCatalogo);

        var cluster = catalogo.ObtenerClusterPorId(datosFusionarItem.IdCluster);

        if (cluster is null) return;

        bool fusionado = cluster.FusionarCanonico();
        if (!fusionado) return;

        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.FusionarCluster,
            $"Se fusionó el canónico del cluster '{cluster.Canonico?.Titulo}' con {cluster.PertenecientesCluster.Count()} ítems.");
    }

    public bool ItemEstaEnCluster(int idCatalogo, int idItem)
    {
        var catalogo = ObtenerCatalogo(idCatalogo);

        var item = ObtenerItem(catalogo, idItem);

        return catalogo.ObtenerClusterDe(item) is not null;
    }
    
    private record ContextoCluster(
        Catalogo Catalogo,
        Item ItemA,
        Item ItemB,
        Cluster? ClusterA,
        Cluster? ClusterB
    );
}