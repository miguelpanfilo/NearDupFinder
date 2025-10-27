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
        var logsFiltrados = FiltrarAuditoriasPorFecha(fechaInicial, fechaFinal);
        var configuracionDeLineasCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = " | ", 
        };
        using var archivoDeMemoriaVirtual = new MemoryStream();
        using (var writer = new StreamWriter(archivoDeMemoriaVirtual))
        using (var csv = new CsvWriter(writer, configuracionDeLineasCsv))
        {
            csv.WriteField("Fecha y hora");
            csv.WriteField("Usuario");
            csv.WriteField("Acción");
            csv.WriteField("Descripción");
            csv.NextRecord();

            foreach (var log in logsFiltrados)
            {
                csv.WriteField(log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
                csv.WriteField(log.Usuario);
                csv.WriteField(log.Accion.ToString());
                csv.WriteField(log.Detalles);
                csv.NextRecord();
            }

            writer.Flush();
        }

        return archivoDeMemoriaVirtual.ToArray();
    }
    
}