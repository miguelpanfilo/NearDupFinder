using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.ReservasStock;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Clusters;

public class GestorControlClusters
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly GestorAuditoria _gestorAuditoria;
    private readonly IRepositorioCatalogos _repositorioCatalogos;
    private readonly IRepositorioClusters _repoClusters;
    private readonly IRepositorioItems _repoItems;

    public GestorControlClusters(
        GestorCatalogos gestorCatalogos,
        GestorAuditoria gestorAuditoria,
        IRepositorioCatalogos repositorioCatalogos,
        IRepositorioClusters repoClusters,
        IRepositorioItems repoItems)
    {
        _gestorCatalogos       = gestorCatalogos;
        _gestorAuditoria       = gestorAuditoria;
        _repositorioCatalogos  = repositorioCatalogos;
        _repoClusters          = repoClusters;
        _repoItems             = repoItems;
    }
    
    private record ContextoCluster(
        Catalogo Catalogo,
        Item ItemA,
        Item ItemB,
        Cluster? ClusterA,
        Cluster? ClusterB
    );

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
        var clusterA = ObtenerCluster(catalogo, itemA);
        var clusterB = ObtenerCluster(catalogo, itemB);
   
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

    public IReadOnlyCollection<DatosPublicosCluster> ObtenerClustersDtoCatalogo(int id)
    {
        List<DatosPublicosCluster> clustersDto = new List<DatosPublicosCluster>();
        var catalogo =  ObtenerCatalogo(id);
        IReadOnlyCollection<Cluster> clusters = ObtenerClustersCatalogo(catalogo);
        foreach (var cluster in clusters)
        {
            clustersDto.Add(DatosPublicosCluster.FromEntity(cluster));
        }
        
        return clustersDto;
    }

    public List<Cluster> ObtenerClustersCatalogo(Catalogo catalogo)
    {
        return catalogo.Clusters.ToList();
    }

    private Cluster? ObtenerCluster(Catalogo c, Item a) =>
        c.ObtenerClusterDe(a);
        

    private static bool SonMismoCluster(Cluster? a, Cluster? b) =>
        a is not null && b is not null && a.Id == b.Id;

    private bool AgregarItem(Catalogo catalogo, Cluster destino, Item itemAgregar, Item itemOtro)
    {
        catalogo.AgregarItemACluster(destino, itemAgregar);
        
        _repoItems.AsignarCluster(itemAgregar.Id, destino.Id);
        _repoItems.GuardarCambios();

        LogConfirmacion(catalogo, itemOtro, itemAgregar);
        return true;
    }

    private bool Fusionar(Catalogo catalogo, Cluster a, Cluster b, Item itemA, Item itemB)
    {
        var itemsDeB = b.PertenecientesCluster.ToList();

        foreach (var it in itemsDeB)
            catalogo.AgregarItemACluster(a, it);

        foreach (var it in itemsDeB)
            _repoItems.AsignarCluster(it.Id, a.Id);
        _repoItems.GuardarCambios();

        catalogo.EliminarCluster(b);

        _repoClusters.Eliminar(b);
        _repoClusters.GuardarCambios();

        LogConfirmacion(catalogo, itemA, itemB);
        return true;
    }

    private bool Crear(Catalogo catalogo, Item itemA, Item itemB)
    {
        catalogo.CrearCluster(new HashSet<Item> { itemA, itemB });
        
        var cluster = catalogo.ObtenerClusterDe(itemA)
                      ?? throw new ExcepcionCatalogo("No se pudo obtener el cluster recién creado.");
        
        _repoClusters.Agregar(cluster);
        _repoClusters.GuardarCambios();

        _repoItems.AsignarCluster(itemA.Id, cluster.Id);
        _repoItems.AsignarCluster(itemB.Id, cluster.Id);
        _repoItems.GuardarCambios();

        LogConfirmacion(catalogo, itemA, itemB);
        return true;
    }

    private void LogConfirmacion(Catalogo catalogo, Item a, Item b)
    {
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.ConfirmarDuplicado,
            $"Confirmado duplicado en catálogo {catalogo.Id}: '{a.Titulo}' (Id={a.Id}) ↔ '{b.Titulo}' (Id={b.Id})."
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
        
        _repoItems.AsignarCluster(item.Id, null);
        _repoItems.GuardarCambios();

        if (cluster.PertenecientesCluster.Count() < catalogo.CantidadMinimaParaQueClusterExista)
        {
            catalogo.EliminarCluster(cluster);
            _repoClusters.Eliminar(cluster);
            _repoClusters.GuardarCambios();
            return;
        }
        
        if (esCanonico)
        {
            cluster.Canonico = null;
            cluster.ImagenCanonicaBase64 = null;
            cluster.StockMinimoCanonico = null;
            cluster.PrecioCanonico = null;

            _repoClusters.Actualizar(cluster);
            _repoClusters.GuardarCambios();
        }
    }

    public void FusionarItemsEnElCluster(DatosFusionarItems datosFusionarItem)
    {
        var catalogo = ObtenerCatalogo(datosFusionarItem.IdCatalogo);
        var cluster  = catalogo.ObtenerClusterPorId(datosFusionarItem.IdCluster)
                       ?? throw new ExcepcionCatalogo("Cluster inexistente.");

        bool fusionado = cluster.FusionarCanonico();
        if (fusionado)
        {
            if (cluster.Canonico is not null)
            {
                if (cluster.ImagenCanonicaBase64 is not null)
                    cluster.Canonico.EditarImagen(cluster.ImagenCanonicaBase64);
                else
                    cluster.Canonico.EditarImagen(null);

                if (cluster.PrecioCanonico.HasValue)
                    cluster.Canonico.EditarPrecio(cluster.PrecioCanonico.Value);
            }

            _repoClusters.Actualizar(cluster);
            _repoClusters.GuardarCambios();

            _gestorAuditoria.RegistrarLog(
                EntradaDeLog.AccionLog.FusionarCluster,
                $"Se fusionó el canónico del cluster '{cluster.Canonico?.Titulo}' con {cluster.PertenecientesCluster.Count()} ítems.");
        }
    }

    
    public void ReservarStockEnCluster(int idcatalogo,int idItemCanonico, int cantidad)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(idcatalogo);
        
        var item = ObtenerItem(catalogo!,idItemCanonico);
        
        var cluster  = catalogo!.ObtenerClusterDe(item)
                       ?? throw new ExcepcionCatalogo("Cluster inexistente.");

        bool reservo = GestorReservas.Aplicar(cluster, cantidad);
        if (reservo)
        {

            _repoClusters.Actualizar(cluster);
            _repoClusters.GuardarCambios();

            _gestorAuditoria.RegistrarLog(
                EntradaDeLog.AccionLog.FusionarCluster,
                $"Se reservó el stock del Item '{cluster.Canonico?.Titulo}' (Cluster Id={cluster.Id}).");
        }
    }

    public int? StockDelCluster(int caltaogoId, int itemId)
    {
        var catalogo = ObtenerCatalogo(caltaogoId);
        var item = ObtenerItem(catalogo, itemId);
        var cluster = ObtenerCluster(catalogo,item) 
                      ?? throw new ExcepcionCatalogo($"El ítem (Id={item.Id}) no pertenece a ningún cluster.");;
        
        return cluster.StockActual;
    }

    public bool ItemEstaEnCluster(int idCatalogo, int idItem)
    {
        var catalogo = ObtenerCatalogo(idCatalogo);

        var item = ObtenerItem(catalogo, idItem);

        return catalogo.ObtenerClusterDe(item) is not null;
    }
    
    public void ConfigurarCanonicoCluster(DatosConfigurarCanonicoCluster datos)
    {
        var catalogo = ObtenerCatalogo(datos.IdCatalogo);
        var cluster = catalogo.ObtenerClusterPorId(datos.IdCluster)
                      ?? throw new ExcepcionCatalogo("Cluster inexistente.");

        if (datos.StockMinimo is < 0)
            throw new ExcepcionItem("El stock mínimo no puede ser negativo.");

        if (datos.PrecioSeleccionado is < 0)
            throw new ExcepcionItem("El precio no puede ser negativo.");

        if (datos.PrecioSeleccionado is int precioSel &&
            cluster.PertenecientesCluster.All(i => i.Precio != precioSel))
        {
            throw new ExcepcionItem("El precio seleccionado no pertenece a ningún ítem del cluster.");
        }

        string? imagenBase64Seleccionada = null;

        if (datos.IdItemImagenSeleccionada is int idItemImagen)
        {
            var itemImagen = cluster.PertenecientesCluster
                                 .SingleOrDefault(i => i.Id == idItemImagen)
                             ?? throw new ExcepcionItem($"El ítem (Id={idItemImagen}) no pertenece al cluster.");

            imagenBase64Seleccionada = itemImagen.ImagenBase64;
        }

        cluster.ConfigurarCanonico(
            imagenBase64: imagenBase64Seleccionada,
            stockMinimo: datos.StockMinimo,
            precio: datos.PrecioSeleccionado
        );

        _repoClusters.Actualizar(cluster);
        _repoClusters.GuardarCambios();
    }

}