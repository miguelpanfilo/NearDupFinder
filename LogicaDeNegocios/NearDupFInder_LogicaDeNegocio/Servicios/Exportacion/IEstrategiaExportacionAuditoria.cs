using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Exportacion
{
    public interface IEstrategiaExportacionAuditoria
    {
        byte[] Exportar(List<EntradaDeLog> auditorias, DateTime fechaInicial, DateTime fechaFinal);
    }
}