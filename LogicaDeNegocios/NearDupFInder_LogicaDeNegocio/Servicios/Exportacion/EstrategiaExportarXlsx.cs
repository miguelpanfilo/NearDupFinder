using ClosedXML.Excel;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Exportacion
{
    public class EstrategiaExportarXlsx : IEstrategiaExportacionAuditoria
    {
        public byte[] Exportar(List<EntradaDeLog> auditorias, DateTime fechaInicial, DateTime fechaFinal)
        {
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

            int filaActual = 2;
            foreach (var log in auditorias)
            {
                hojaVirtualExcel.Cell(filaActual, 1).Value = log.Timestamp;
                hojaVirtualExcel.Cell(filaActual, 1).Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";
                hojaVirtualExcel.Cell(filaActual, 2).Value = log.Usuario;
                hojaVirtualExcel.Cell(filaActual, 3).Value = log.Accion.ToString();
                hojaVirtualExcel.Cell(filaActual, 4).Value = log.Detalles;
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
}
