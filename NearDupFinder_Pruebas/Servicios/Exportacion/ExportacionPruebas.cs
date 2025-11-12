using ClosedXML.Excel;
using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFinder_Pruebas.Utilidades;


namespace NearDupFinder_Pruebas.Servicios
{
    [TestClass]
    public class ExportacionPruebas
    {
        private GestorExportacionAuditoria _gestorExportacion = null!;
        private GestorAuditoria _gestorAuditoria = null!;
        private SqlContext _contexto = null!;
        private IRepositorioAuditorias _repoAuditorias = null!;

        [TestInitialize]
        public void Setup()
        {
            var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_AuditoriaExportacion");
            _contexto = SqlContextFactoryPruebas.CrearContexto(opciones);
            SqlContextFactoryPruebas.LimpiarBaseDeDatos(_contexto);

            _repoAuditorias = new RepositorioAuditorias(_contexto);

            var sesionUsuario = new SesionUsuarioActual();
            sesionUsuario.Asignar("tester@correo.com");

            _gestorAuditoria = new GestorAuditoria(_repoAuditorias, sesionUsuario);

            _gestorAuditoria.RegistrarLogManual(
                new DateTime(2025, 10, 25, 10, 0, 0),
                "admin@test.com",
                EntradaDeLog.AccionLog.AltaUsuario,
                "Creación de usuario admin"
            );

            _gestorAuditoria.RegistrarLogManual(
                new DateTime(2025, 10, 27, 15, 30, 0),
                "user@test.com",
                EntradaDeLog.AccionLog.EditarItem,
                "Modificación de item prueba"
            );

            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarCsv()
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            _contexto?.Dispose();
        }

        [TestMethod]
        public void FiltrarAuditoriasPorFecha_DeberiaDevolverSoloLogsEnRango()
        {
            var desde = new DateTime(2025, 10, 26);
            var hasta = new DateTime(2025, 10, 28);

            var resultado = _gestorExportacion.FiltrarAuditoriasPorFecha(desde, hasta);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("user@test.com", resultado[0].Usuario);
        }
        [TestMethod]
        public void FiltrarAuditoriasPorFecha_DeberiaIncluirFechasLimite()
        {
            var desde = new DateTime(2025, 10, 25, 10, 0, 0); 
            var hasta = new DateTime(2025, 10, 27, 15, 30, 0); 

            var resultado = _gestorExportacion.FiltrarAuditoriasPorFecha(desde, hasta);

            Assert.AreEqual(2, resultado.Count, "Ambos logs deberían incluirse cuando coinciden con los límites del rango.");
        }

        [TestMethod]
        public void FiltrarAuditoriasPorFecha_SinLogsEnRango_DeberiaDevolverListaVacia()
        {
            var desde = new DateTime(2025, 11, 1);
            var hasta = new DateTime(2025, 11, 2);

            var resultado = _gestorExportacion.FiltrarAuditoriasPorFecha(desde, hasta);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count, "No debería devolver logs fuera del rango.");
        }
        [TestMethod]
        public void FiltrarAuditoriasPorFecha_DeberiaOrdenarPorFechaAscendente()
        {
            var desde = new DateTime(2025, 10, 20);
            var hasta = new DateTime(2025, 10, 30);

            var resultado = _gestorExportacion.FiltrarAuditoriasPorFecha(desde, hasta);

            Assert.IsTrue(resultado.SequenceEqual(resultado.OrderBy(l => l.Timestamp)),
                "Los logs deberían estar ordenados por fecha ascendente.");
        }

        [TestMethod]
        public void ExportarAuditorias_Csv_DeberiaGenerarArchivoConEncabezadosYDelimitadorCorrecto()
        {
            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarCsv()
            );

            var bytes = _gestorExportacion.ExportarAuditorias(
                new DateTime(2025, 10, 25),
                new DateTime(2025, 10, 28)
            );

            var contenido = System.Text.Encoding.UTF8.GetString(bytes);

            StringAssert.Contains(contenido, "Fecha y hora | Usuario | Acción | Descripción");
            StringAssert.Contains(contenido, "user@test.com");
            Assert.IsTrue(contenido.Contains("|"), "El archivo debe usar '|' como delimitador.");
        }

        [TestMethod]
        public void GenerarCsvBytes_SinLogs_DeberiaContenerSoloEncabezados()
        {
            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarCsv()
            );

            var bytes = _gestorExportacion.ExportarAuditorias(
                new DateTime(2025, 11, 1),
                new DateTime(2025, 11, 2)
            );

            var contenido = System.Text.Encoding.UTF8.GetString(bytes);
            StringAssert.Contains(contenido, "Fecha y hora | Usuario | Acción | Descripción");
            Assert.IsFalse(contenido.Contains("user@test.com"), "No debe contener datos de logs fuera del rango.");
        }


        [TestMethod]
        public void GenerarXlsxBytes_DeberiaCrearArchivoConHojaYDatosCorrectos()
        {
            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarXlsx()
            );

            var bytes = _gestorExportacion.ExportarAuditorias(
                new DateTime(2025, 10, 26),
                new DateTime(2025, 10, 28)
            );

            Assert.IsTrue(bytes.Length > 0, "El archivo XLSX no debería estar vacío.");

            using var memoria = new MemoryStream(bytes);
            using var hojaExcel = new XLWorkbook(memoria);
            var hoja = hojaExcel.Worksheets.First();

            Assert.AreEqual("Auditorías", hoja.Name);
            Assert.AreEqual("Usuario", hoja.Cell(1, 2).Value);
            Assert.AreEqual("user@test.com", hoja.Cell(2, 2).Value);
        }

        [TestMethod]
        public void GenerarXlsxBytes_SinLogs_DeberiaCrearSoloEncabezados()
        {
            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarXlsx()
            );

            var archivoDatos = _gestorExportacion.ExportarAuditorias(
                new DateTime(2025, 11, 1),
                new DateTime(2025, 11, 2)
            );

            using var memoria = new MemoryStream(archivoDatos);
            using var hojaExcel = new XLWorkbook(memoria);
            var hoja = hojaExcel.Worksheets.First();

            Assert.AreEqual("Auditorías", hoja.Name);
            Assert.AreEqual("Usuario", hoja.Cell(1, 2).Value);
            Assert.IsTrue(hoja.Cell(2, 1).IsEmpty(), "No debería haber datos debajo del encabezado.");
        }

        [TestMethod]
        public void GenerarXlsxBytes_DeberiaUsarFormatoDeFechaCorrecto()
        {
            _gestorExportacion = new GestorExportacionAuditoria(
                _gestorAuditoria,
                new EstrategiaExportarXlsx()
            );

            var archivoDeDatosExcel = _gestorExportacion.ExportarAuditorias(
                new DateTime(2025, 10, 25),
                new DateTime(2025, 10, 28)
            );

            using var memoriaVirtual = new MemoryStream(archivoDeDatosExcel);
            using var hojaExcel = new XLWorkbook(memoriaVirtual);
            var hoja = hojaExcel.Worksheets.First();

            var formato = hoja.Cell(2, 1).Style.DateFormat.Format;
            Assert.AreEqual("dd/MM/yyyy HH:mm:ss", formato);
        }

        
    }
}
