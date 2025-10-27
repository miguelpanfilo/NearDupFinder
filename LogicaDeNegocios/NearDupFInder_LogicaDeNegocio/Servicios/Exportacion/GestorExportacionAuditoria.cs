using System.Globalization;
using ClosedXML.Excel;
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
     public byte[] GenerarXlsxBytes(DateTime fechaInicial, DateTime fechaFinal)
    {
        var audtoriasLogueadas = FiltrarAuditoriasPorFecha(fechaInicial, fechaFinal);

        using var hojaExcel = new XLWorkbook();
        var hojaVirtualExcel = hojaExcel.Worksheets.Add("Auditorías");

        hojaVirtualExcel.Cell(1, 1).Value = "Fecha y hora";
        hojaVirtualExcel.Cell(1, 2).Value = "Usuario";
        hojaVirtualExcel.Cell(1, 3).Value = "Acción";
        hojaVirtualExcel.Cell(1, 4).Value = "Descripción";

        var rangoEncabezado = hojaVirtualExcel.Range("A1:D1");
        rangoEncabezado.Style.Font.Bold = true;
        rangoEncabezado.Style.Fill.BackgroundColor = XLColor.LightGray;
        rangoEncabezado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoEncabezado.Style.Border.BottomBorder = XLBorderStyleValues.Medium;

        const int columnaFecha = 1;
        const int columnaUsuario = 2;
        const int columnaAccion = 3;
        const int columnaDescripcion = 4;

        int filaActual = 2; 

        foreach (var log in audtoriasLogueadas)
        {
            hojaVirtualExcel.Cell(filaActual, columnaFecha).Value = log.Timestamp;
            hojaVirtualExcel.Cell(filaActual, columnaFecha).Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";

            hojaVirtualExcel.Cell(filaActual, columnaUsuario).Value = log.Usuario;
            hojaVirtualExcel.Cell(filaActual, columnaAccion).Value = log.Accion.ToString();
            hojaVirtualExcel.Cell(filaActual, columnaDescripcion).Value = log.Detalles;

            filaActual++;
        }

        var tablaDeExcel = hojaVirtualExcel.Range(1, 1, filaActual - 1, 4);
        tablaDeExcel.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tablaDeExcel.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        hojaVirtualExcel.Columns().AdjustToContents();

        hojaVirtualExcel.SheetView.FreezeRows(1);

        using var archivoEnMemoriaRam = new MemoryStream();
        hojaExcel.SaveAs(archivoEnMemoriaRam);
        return archivoEnMemoriaRam.ToArray();
    }


}