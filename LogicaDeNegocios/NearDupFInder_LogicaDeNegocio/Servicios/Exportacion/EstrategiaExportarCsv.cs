using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Exportacion
{
    public class EstrategiaExportarCsv : IEstrategiaExportacionAuditoria
    {
        public byte[] Exportar(List<EntradaDeLog> auditorias, DateTime fechaInicial, DateTime fechaFinal)
        {
            var configuracionDeLineasCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = " | ",
            };

            MemoryStream archivoDeMemoriaVirtual = new MemoryStream();
            StreamWriter escritorDelArchivoVirtual = new StreamWriter(archivoDeMemoriaVirtual);
            CsvWriter escritorDelCsvNuevo = new CsvWriter(escritorDelArchivoVirtual, configuracionDeLineasCsv);

            using (archivoDeMemoriaVirtual)
            using (escritorDelArchivoVirtual)
            using (escritorDelCsvNuevo)
            {
                EscribirEncabezados(escritorDelCsvNuevo);
                EscribirLineas(escritorDelCsvNuevo, auditorias);

                escritorDelArchivoVirtual.Flush();
            }

            return archivoDeMemoriaVirtual.ToArray();
        }
        private void EscribirEncabezados(CsvWriter escritorDelCsvNuevo)
        {
            escritorDelCsvNuevo.WriteField("Fecha y hora");
            escritorDelCsvNuevo.WriteField("Usuario");
            escritorDelCsvNuevo.WriteField("Acción");
            escritorDelCsvNuevo.WriteField("Descripción");
            escritorDelCsvNuevo.NextRecord();
        }
        private void EscribirLineas(CsvWriter escritorDelCsvNuevo, List<EntradaDeLog> auditorias)
        {
            foreach (var log in auditorias)
            {
                escritorDelCsvNuevo.WriteField(log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
                escritorDelCsvNuevo.WriteField(log.Usuario);
                escritorDelCsvNuevo.WriteField(log.Accion.ToString());
                escritorDelCsvNuevo.WriteField(log.Detalles);
                escritorDelCsvNuevo.NextRecord();
            }
        }


    }
}