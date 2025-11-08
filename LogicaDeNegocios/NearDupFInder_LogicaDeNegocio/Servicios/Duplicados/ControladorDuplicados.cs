using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

public class ControladorDuplicados(
    GestorAuditoria gestorAuditoria,
    GestorDuplicados gestorDuplicados,
    GestorCatalogos gestorCatalogos,
    GestorControlClusters gestorControlClusters,
    List<ParDuplicado> duplicadosGlobales)
{
    public void ProcesarDuplicados(int idCatalogo, int idItem)
    {
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

        Item item = catalogo.ObtenerItemPorId(idItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={idItem}).");

        List<ParDuplicado> duplicados = DetectarDuplicados(item, catalogo);
        AgregarDuplicadosADuplicadosGlobales(duplicados);
    }
    
    public void ActualizarEstadoDuplicadosEnCatalogo(Catalogo catalogo)
    {
        foreach (Item item in catalogo.Items)
        {
            bool tieneDuplicados = duplicadosGlobales.Any(duplicado =>
                duplicado.ItemAComparar.Id == item.Id || duplicado.ItemPosibleDuplicado.Id == item.Id);
            item.EstadoDuplicado = tieneDuplicados;
        }
    }

    public void ActualizarDuplicadosPara(DatosActualizarDuplicados datosActualizarDuplicados)
    {
        
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(datosActualizarDuplicados.IdCatalogo)
                            ?? throw new ExcepcionCatalogo(
                                $"Catálogo no encontrado (Id={datosActualizarDuplicados.IdCatalogo}).");

        Item itemEditado = catalogo.ObtenerItemPorId(datosActualizarDuplicados.IdItem)
                           ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datosActualizarDuplicados.IdItem}).");

        EliminarDuplicadosPrevios(itemEditado);
        
        gestorControlClusters.BorrarItemDelCluster(new DatosRemoverItemCluster(itemEditado.Id, catalogo.Id));

        List<ParDuplicado> nuevosDuplicados = DetectarDuplicados(itemEditado, catalogo);
        AgregarDuplicadosADuplicadosGlobales(nuevosDuplicados);

        ActualizarEstadoDuplicadosEnCatalogo(catalogo);
    }

    public void ConfirmarParDuplicado(DatosDuplicados datosDuplicados)
    {
        bool confirmado = gestorControlClusters.ConfirmarCluster(datosDuplicados);
        if (confirmado)
        {
            DescartarParDuplicado(datosDuplicados);
        };
    }
    public void DescartarParDuplicado(DatosDuplicados datos)
    {
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(datos.IdCatalogo)
                            ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datos.IdCatalogo}).");

        Item itemA = catalogo.ObtenerItemPorId(datos.IdItemAComparar)
                     ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemAComparar}).");

        Item itemB = catalogo.ObtenerItemPorId(datos.IdItemPosibleDuplicado)
                     ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemPosibleDuplicado}).");

        int removidos = duplicadosGlobales.RemoveAll(p =>
            (p.ItemAComparar.Id == itemA.Id && p.ItemPosibleDuplicado.Id == itemB.Id) ||
            (p.ItemAComparar.Id == itemB.Id && p.ItemPosibleDuplicado.Id == itemA.Id));

        if (removidos == 0)
            throw new ExcepcionDuplicado($"El par (A={itemA.Id}, B={itemB.Id}) no estaba en la lista de duplicados.");


        itemA.EstadoDuplicado = ExisteParConItem(itemA.Id);
        itemB.EstadoDuplicado = ExisteParConItem(itemB.Id);


        gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DescartarDuplicado,
            $"Se descartó el par de posibles duplicados: '{itemA.Titulo}' (Id={itemA.Id}) y '{itemB.Titulo}' (Id={itemB.Id}) en catálogo Id={catalogo.Id}."
        );
    }
    
    public void EliminarDuplicadosPrevios(Item item)
    {
        var duplicadosABorrar = duplicadosGlobales
            .Where(duplicado => duplicado.ItemAComparar.Id == item.Id || duplicado.ItemPosibleDuplicado.Id == item.Id)
            .ToList();
        foreach (ParDuplicado duplicado in duplicadosABorrar)
            duplicadosGlobales.Remove(duplicado);
    }

    public List<DatosParDuplicado> ObtenerDuplicadosOrdenados()
    {
        List<ParDuplicado> listaDuplicadosOrdenada = duplicadosGlobales
            .OrderByDescending(d => d.Score)
            .ToList();
        
        List<DatosParDuplicado> listaDtosParDuplicado = [];
        foreach (ParDuplicado duplicado in listaDuplicadosOrdenada)
            listaDtosParDuplicado.Add(DatosParDuplicado.FromEntity(duplicado));
        
        return listaDtosParDuplicado;
    }

    private void AgregarDuplicadosADuplicadosGlobales(IEnumerable<ParDuplicado> duplicados)
    {
        foreach (ParDuplicado dup in duplicados)
        {
            duplicadosGlobales.Add(dup);
            dup.ItemAComparar.EstadoDuplicado = true;
            dup.ItemPosibleDuplicado.EstadoDuplicado = true;
        }
    }

    private bool ExisteParConItem(int itemId) =>
        duplicadosGlobales.Any(p => p.ItemAComparar.Id == itemId || p.ItemPosibleDuplicado.Id == itemId);

    private List<ParDuplicado> DetectarDuplicados(Item itemAComparar, Catalogo catalogo)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var duplicados = gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);
        stopwatch.Stop();
        gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DeteccionDuplicados,
            $"Detección de duplicados para item '{itemAComparar.Titulo}' en catálogo '{catalogo.Titulo}' completada en {stopwatch.ElapsedMilliseconds} ms."
        );
        
        return duplicados;
    }
}