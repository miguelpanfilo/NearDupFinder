using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlDuplicados;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorDuplicados;

namespace NearDupFinder_LogicaDeNegocio.Servicios;

public class GestorControlDuplicados
{
    private readonly GestorAuditoria _gestorAuditoria;
    private readonly GestorDuplicados _gestorDuplicados;
    private readonly List<ParDuplicado> _duplicadosGlobales;
    private readonly GestorCatalogos _gestorCatalogos;

    public GestorControlDuplicados(
        GestorAuditoria gestorAuditoria,
        GestorDuplicados gestorDuplicados,
        GestorCatalogos gestorCatalogos,
        List<ParDuplicado> duplicadosGlobales)
    {
        _gestorAuditoria = gestorAuditoria;
        _gestorCatalogos = gestorCatalogos;
        _gestorDuplicados = gestorDuplicados;
        _duplicadosGlobales = duplicadosGlobales;
    }

    public void ProcesarDuplicadosPorAlta(int idCatalogo, int idItem)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(idCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={idCatalogo}).");

        var item = catalogo.ObtenerItemPorId(idItem)
                   ?? throw new ExcepcionItem($"Ítem no encontrado (Id={idItem}).");

        var duplicados = DetectarDuplicadosConAuditoria(item, catalogo);
        AgregarDuplicadosADuplicadosGlobales(duplicados);
    }

    public void ActualizarDuplicadosPara(DatosActualizarDuplicados datosActualizarDuplicados)
    {
        
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datosActualizarDuplicados.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datosActualizarDuplicados.IdCatalogo}).");

        var itemEditado = catalogo.ObtenerItemPorId(datosActualizarDuplicados.IdItem)
                          ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datosActualizarDuplicados.IdItem}).");

        EliminarDuplicadosPrevios(itemEditado);
        catalogo.QuitarItemDeCluster(itemEditado);

        var nuevosDuplicados = DetectarDuplicadosConAuditoria(itemEditado, catalogo);
        AgregarDuplicadosADuplicadosGlobales(nuevosDuplicados);

        ActualizarEstadoDuplicadosEnCatalogo(catalogo);
    }
    
    public List<ParDuplicado> ObtenerDuplicadosOrdenados()
    {
        return _duplicadosGlobales
            .OrderByDescending(d => d.Score)
            .ToList();
    }

    private void AgregarDuplicadosADuplicadosGlobales(IEnumerable<ParDuplicado>? duplicados)
    {
        if (duplicados == null) return;
        foreach (var dup in duplicados)
        {
            _duplicadosGlobales.Add(dup);
            dup.ItemAComparar.EstadoDuplicado = true;
            dup.ItemPosibleDuplicado.EstadoDuplicado = true;
        }
    }

    public void EliminarDuplicadosPrevios(Item item)
    {
        var duplicadosABorrar = _duplicadosGlobales
            .Where(d => d.ItemAComparar.Id == item.Id || d.ItemPosibleDuplicado.Id == item.Id)
            .ToList();
        foreach (var duplicado in duplicadosABorrar)
            _duplicadosGlobales.Remove(duplicado);
    }

    public void ActualizarEstadoDuplicadosEnCatalogo(Catalogo catalogo)
    {
        foreach (var item in catalogo.Items)
        {
            bool tieneDuplicados = _duplicadosGlobales.Any(d =>
                d.ItemAComparar.Id == item.Id || d.ItemPosibleDuplicado.Id == item.Id);
            item.EstadoDuplicado = tieneDuplicados;
        }
    }

    public void DescartarParDuplicado(DatosDuplicados datos)
    {
        var catalogo = _gestorCatalogos.ObtenerCatalogoPorId(datos.IdCatalogo)
                       ?? throw new ExcepcionCatalogo($"Catálogo no encontrado (Id={datos.IdCatalogo}).");

        var itemA = catalogo.ObtenerItemPorId(datos.IdItemAComparar)
                    ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemAComparar}).");

        var itemB = catalogo.ObtenerItemPorId(datos.IdItemPosibleDuplicado)
                    ?? throw new ExcepcionItem($"Ítem no encontrado (Id={datos.IdItemPosibleDuplicado}).");

        var removidos = _duplicadosGlobales.RemoveAll(p =>
            (p.ItemAComparar.Id == itemA.Id && p.ItemPosibleDuplicado.Id == itemB.Id) ||
            (p.ItemAComparar.Id == itemB.Id && p.ItemPosibleDuplicado.Id == itemA.Id));

        if (removidos == 0)
            throw new ExcepcionDuplicado($"El par (A={itemA.Id}, B={itemB.Id}) no estaba en la lista de duplicados.");


        itemA.EstadoDuplicado = ExisteParConItem(itemA.Id);
        itemB.EstadoDuplicado = ExisteParConItem(itemB.Id);


        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DescartarDuplicado,
            $"Se descartó el par de posibles duplicados: '{itemA.Titulo}' (Id={itemA.Id}) y '{itemB.Titulo}' (Id={itemB.Id}) en catálogo Id={catalogo.Id}."
        );
    }
    
    private bool ExisteParConItem(int itemId) =>
        _duplicadosGlobales.Any(p => p.ItemAComparar.Id == itemId || p.ItemPosibleDuplicado.Id == itemId);

    private List<ParDuplicado> DetectarDuplicadosConAuditoria(Item itemAComparar, Catalogo? catalogo)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var duplicados = _gestorDuplicados.DetectarDuplicados(itemAComparar, catalogo);
        stopwatch.Stop();
        _gestorAuditoria.RegistrarLog(
            EntradaDeLog.AccionLog.DeteccionDuplicados,
            $"Detección de duplicados para item '{itemAComparar.Titulo}' en catálogo '{catalogo?.Titulo}' completada en {stopwatch.ElapsedMilliseconds} ms."
        );
        return duplicados;
    }
}