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
        return _gestorAuditoria.ObtenerTodos()
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
        using (var escritorDelArchivoVirtual = new StreamWriter(archivoDeMemoriaVirtual))
        using (var escritorDelCsvNuevo = new CsvWriter(escritorDelArchivoVirtual, configuracionDeLineasCsv))
        {
            escritorDelCsvNuevo.WriteField("Fecha y hora");
            escritorDelCsvNuevo.WriteField("Usuario");
            escritorDelCsvNuevo.WriteField("Acción");
            escritorDelCsvNuevo.WriteField("Descripción");
            escritorDelCsvNuevo.NextRecord();

            foreach (var log in logsFiltrados)
            {
                escritorDelCsvNuevo.WriteField(log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
                escritorDelCsvNuevo.WriteField(log.Usuario);
                escritorDelCsvNuevo.WriteField(log.Accion.ToString());
                escritorDelCsvNuevo.WriteField(log.Detalles);
                escritorDelCsvNuevo.NextRecord();
            }

            escritorDelArchivoVirtual.Flush();
        }

        return archivoDeMemoriaVirtual.ToArray();
    }
     public byte[] GenerarXlsxBytes(DateTime fechaInicial, DateTime fechaFinal)
    {
        var audtoriasLogueadas = FiltrarAuditoriasPorFecha(fechaInicial, fechaFinal);

        using var hojaExcel = new XLWorkbook();
        var hojaVirtualExcel = hojaExcel.Worksheets.Add("Auditorías");

        const int colEncabezado = 1;
        const int columnaFechaEncabezado = 1;
        const int columnaUsuarioEncabezado = 2;
        const int columnaAccionEncabezado = 3;
        const int columnaDescripcionEncabezado = 4;

        hojaVirtualExcel.Cell(colEncabezado, columnaFechaEncabezado).Value = "Fecha y hora";
        hojaVirtualExcel.Cell(colEncabezado, columnaUsuarioEncabezado).Value = "Usuario";
        hojaVirtualExcel.Cell(colEncabezado, columnaAccionEncabezado).Value = "Acción";
        hojaVirtualExcel.Cell(colEncabezado, columnaDescripcionEncabezado).Value = "Descripción";

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

        const int filaEncabezadoInmutable=1;
        hojaVirtualExcel.SheetView.FreezeRows(filaEncabezadoInmutable);

        using var archivoEnMemoriaRam = new MemoryStream();
        hojaExcel.SaveAs(archivoEnMemoriaRam);
        return archivoEnMemoriaRam.ToArray();
    }


}