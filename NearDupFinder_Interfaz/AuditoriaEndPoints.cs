/*using NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;

namespace NearDupFinder_Interfaz;

public static class AuditoriaEndpoints
{
    public static void MapAuditoriaExportEndpoints(this WebApplication app)
    {
        app.MapGet("/api/auditorias/export/csv",
            (DateTime desde, DateTime hasta, GestorExportacionAuditoria exportador) =>
            {
                var bytes = exportador.GenerarCsvBytes(desde, hasta);
                var fileName = $"auditorias_{desde:yyyyMMdd}_{hasta:yyyyMMdd}.csv";

                return Results.File(
                    bytes,
                    "text/csv",
                    fileName
                );
            });

        app.MapGet("/api/auditorias/export/xlsx",
            (DateTime desde, DateTime hasta, GestorExportacionAuditoria exportador) =>
            {
                var bytes = exportador.GenerarXlsxBytes(desde, hasta);
                var fileName = $"auditorias_{desde:yyyyMMdd}_{hasta:yyyyMMdd}.xlsx";

                return Results.File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            });
    }
}*/