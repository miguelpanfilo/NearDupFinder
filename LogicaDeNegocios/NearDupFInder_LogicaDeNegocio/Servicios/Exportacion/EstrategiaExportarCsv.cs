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
            using var archivoDeMemoriaVirtual = new MemoryStream();
            using (var escritorDelArchivoVirtual = new StreamWriter(archivoDeMemoriaVirtual))
            using (var escritorDelCsvNuevo = new CsvWriter(escritorDelArchivoVirtual, configuracionDeLineasCsv))
            {
                escritorDelCsvNuevo.WriteField("Fecha y hora");
                escritorDelCsvNuevo.WriteField("Usuario");
                escritorDelCsvNuevo.WriteField("Acción");
                escritorDelCsvNuevo.WriteField("Descripción");
                escritorDelCsvNuevo.NextRecord();

                foreach (var log in auditorias)
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

    }
}