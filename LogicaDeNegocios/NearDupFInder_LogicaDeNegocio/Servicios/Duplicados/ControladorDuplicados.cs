using System.Diagnostics;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlClusters;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;

public class ControladorDuplicados(
    GestorAuditoria gestorAuditoria,
    GestorDuplicados gestorDuplicados,
    GestorCatalogos gestorCatalogos,
    GestorControlClusters gestorControlClusters,
    IRepositorioDuplicados repositorioDuplicados)
{
    public void ProcesarDuplicados(int idCatalogo, int idItem)
    {
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

        Item item = catalogo.ObtenerItemPorId(idItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={idItem}).");

        List<ParDuplicado> duplicados = DetectarDuplicados(item, catalogo);
        AgregarDuplicados(duplicados);
        ActualizarEstadoDuplicadosEnCatalogo(catalogo);
    }
    
    public void ActualizarEstadoDuplicadosEnCatalogo(Catalogo catalogo)
    {
        var paresDelCatalogo = repositorioDuplicados.ObtenerListaDeDuplicados()
            .Where(p => p.IdCatalogo == catalogo.Id)
            .ToList();

        foreach (var item in catalogo.Items)
        {
            item.EstadoDuplicado = paresDelCatalogo.Any(parDuplicado =>
                parDuplicado.ItemAComparar.Id == item.Id || parDuplicado.ItemPosibleDuplicado.Id == item.Id);
        }
    }

    public void ActualizarDuplicadosPara(DatosActualizarDuplicados datosActualizarDuplicados)
    {
        
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(datosActualizarDuplicados.IdCatalogo)
                            ?? throw new ExcepcionCatalogo(
                                $"Catálogo no encontrado (Id={datosActualizarDuplicados.IdCatalogo}).");

        Item itemEditado = catalogo.ObtenerItemPorId(datosActualizarDuplicados.IdItem)
                           ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datosActualizarDuplicados.IdItem}).");

        EliminarDuplicadosPrevios(itemEditado, catalogo.Id);

        gestorControlClusters.BorrarItemDelCluster(new DatosRemoverItemCluster(itemEditado.Id, catalogo.Id));

        List<ParDuplicado> nuevosDuplicados = DetectarDuplicados(itemEditado, catalogo);
        AgregarDuplicados(nuevosDuplicados);
        ActualizarEstadoDuplicadosEnCatalogo(catalogo);
    }

    public void ConfirmarParDuplicado(DatosDuplicados datosDuplicados)
    {
        bool confirmado = gestorControlClusters.ConfirmarCluster(datosDuplicados);
        if (confirmado)
        {
            DescartarParDuplicado(datosDuplicados);
        }
    }
    public void DescartarParDuplicado(DatosDuplicados datos)
    {
        Catalogo catalogo = gestorCatalogos.ObtenerCatalogoPorId(datos.IdCatalogo)
                            ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datos.IdCatalogo}).");

        Item itemA = catalogo.ObtenerItemPorId(datos.IdItemAComparar)
                     ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemAComparar}).");

        Item itemB = catalogo.ObtenerItemPorId(datos.IdItemPosibleDuplicado)
                     ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemPosibleDuplicado}).");

        var paresPersistidos = repositorioDuplicados.ObtenerListaDeDuplicados()
            .Where(p => p.IdCatalogo == catalogo.Id)
            .ToList();

        ParDuplicado? par = paresPersistidos.FirstOrDefault(parDuplicado =>
            parDuplicado.ItemAComparar.Id == itemA.Id && parDuplicado.ItemPosibleDuplicado.Id == itemB.Id);

        if (par is null)
            throw new ExcepcionDuplicado($"El par (A={itemA.Id}, B={itemB.Id}) no estaba persistido.");

        repositorioDuplicados.EliminarDuplicado(par);

        itemA.EstadoDuplicado = ExisteParConItemEnBd(catalogo.Id, itemA.Id);
        itemB.EstadoDuplicado = ExisteParConItemEnBd(catalogo.Id, itemB.Id);

        gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DescartarDuplicado,
            $"Se descartó el par de posibles duplicados: '{itemA.Titulo}' (Id={itemA.Id}) y '{itemB.Titulo}' (Id={itemB.Id}) en catálogo Id={catalogo.Id}."
        );
    }

    public void EliminarDuplicadosPrevios(Item item, int idCatalogo)
    {
        var paresDelItem = repositorioDuplicados.ObtenerListaDeDuplicados()
            .Where(parDuplicado => parDuplicado.IdCatalogo == idCatalogo && (parDuplicado.ItemAComparar.Id == item.Id || parDuplicado.ItemPosibleDuplicado.Id == item.Id))
            .ToList();

        foreach (var par in paresDelItem)
            repositorioDuplicados.EliminarDuplicado(par);
    }
    
    private bool ExisteParConItemEnBd(int idCatalogo, int idItem)
    {
        return repositorioDuplicados.ObtenerListaDeDuplicados()
            .Any(p => p.IdCatalogo == idCatalogo && (p.ItemAComparar.Id == idItem || p.ItemPosibleDuplicado.Id == idItem));
    }

    public List<DatosParDuplicado> ObtenerDuplicadosOrdenados()
    {
        List<ParDuplicado> listaDuplicadosOrdenada = repositorioDuplicados.ObtenerListaDeDuplicados()
            .OrderByDescending(parDuplicado => parDuplicado.Score)
            .ToList();

        List<DatosParDuplicado> listaDtos = new();
        foreach (ParDuplicado duplicado in listaDuplicadosOrdenada)
            listaDtos.Add(DatosParDuplicado.FromEntity(duplicado));

        return listaDtos;
    }

    private void AgregarDuplicados(IEnumerable<ParDuplicado> duplicados)
    {
        foreach (ParDuplicado parDuplicado in duplicados)
            repositorioDuplicados.AgregarDuplicado(parDuplicado);
    }

    private List<ParDuplicado> DetectarDuplicados(Item itemAComparar, Catalogo catalogo)
    {
        var cronometro = Stopwatch.StartNew();
        List<ParDuplicado> duplicados = gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);
        cronometro.Stop();

        gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DeteccionDuplicados,
            $"Detección de duplicados para item '{itemAComparar.Titulo}' en catálogo '{catalogo.Titulo}' completada en {cronometro.ElapsedMilliseconds} ms."
        );

        return duplicados;
    }
}