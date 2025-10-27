using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;

public class GestorExportacionAuditoria
{
    private readonly GestorAuditoria _gestorAuditoria;

    public GestorExportacionAuditoria(GestorAuditoria gestorAuditoria)
    {
        _gestorAuditoria = gestorAuditoria;
    }

    public List<EntradaDeLog> FiltrarAuditoriasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
    {
        return _gestorAuditoria.ObtenerLogs()
            .Where(l => l.Timestamp >= fechaInicial && l.Timestamp <= fechaFinal)
            .OrderBy(l => l.Timestamp)
            .ToList();
    }
    
    public byte[] GenerarCsvBytes(DateTime fechaInicial, DateTime fechaFinal)
    {
        var logs = FiltrarAuditoriasPorFecha(fechaInicial, fechaFinal);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = " | ", 
        };
        using var ms = new MemoryStream();
        using (var writer = new StreamWriter(ms))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteField("Fecha y hora");
            csv.WriteField("Usuario");
            csv.WriteField("Acción");
            csv.WriteField("Descripción");
            csv.NextRecord();

            foreach (var log in logs)
            {
                csv.WriteField(log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
                csv.WriteField(log.Usuario);
                csv.WriteField(log.Accion.ToString());
                csv.WriteField(log.Detalles);
                csv.NextRecord();
            }

            writer.Flush();
        }

        return ms.ToArray();
    }
    
}