
using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;

public class GestorExportacionAuditoria
{
    private readonly GestorAuditoria _gestorAuditoria;
    private IEstrategiaExportacionAuditoria _estrategia;


    public GestorExportacionAuditoria(GestorAuditoria gestorAuditoria, IEstrategiaExportacionAuditoria estrategia)
    {
        _gestorAuditoria = gestorAuditoria;
        _estrategia = estrategia;
    }
    public void CambiarEstrategia(IEstrategiaExportacionAuditoria nueva)
    {
        _estrategia = nueva;
    }

    public List<EntradaDeLog> FiltrarAuditoriasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
    {
    const int incluirDiaCompleto = 1;
    const int incluirUltimoInstanteDelDia = -1;
        var inicio = fechaInicial.Date;
        var fin = fechaFinal.Date.AddDays(incluirDiaCompleto).AddTicks(incluirUltimoInstanteDelDia);

        return _gestorAuditoria.ObtenerTodos()
            .Where(l => l.Timestamp >= inicio && l.Timestamp <= fin)
            .OrderBy(l => l.Timestamp)
            .ToList();
    }
    
    public byte[] ExportarAuditorias(DateTime fechaInicial, DateTime fechaFinal)
    {
        var logsFiltrados = FiltrarAuditoriasPorFecha(fechaInicial, fechaFinal);
        return _estrategia.Exportar(logsFiltrados, fechaInicial, fechaFinal);
    }


}