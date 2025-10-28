using NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;

namespace NearDupFinder_Interfaz;

public static class ExportacionEndPoints
{
    public static void MapeoAuditoriasExportacionEndpoints(this WebApplication app)
    {
        app.MapGet("/api/auditorias/export/csv",
            (DateTime fechaInicial, DateTime fechaFinal, GestorExportacionAuditoria exportador) =>
            {
                var logsMemoriaCreado = exportador.GenerarCsvBytes(fechaInicial, fechaFinal);
                var nombreCsvCreado = $"auditorias_{fechaInicial:yyyyMMdd}_{fechaFinal:yyyyMMdd}.csv";

                return Results.File(
                    logsMemoriaCreado,
                    "text/csv",
                    nombreCsvCreado
                );
            });

        app.MapGet("/api/auditorias/export/xlsx",
            (DateTime fechaInicial, DateTime fechaFinal, GestorExportacionAuditoria exportador) =>
            {
                var logsMemoriaCreado = exportador.GenerarXlsxBytes(fechaInicial, fechaFinal);
                var nombreXlsxCreado = $"auditorias_{fechaInicial:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx";

                return Results.File(
                    logsMemoriaCreado,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    nombreXlsxCreado
                );
            });
    }
}