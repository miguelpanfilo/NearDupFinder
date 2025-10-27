using NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFinder_Dominio.Clases;


namespace NearDupFinder_Pruebas.Servicios
{
    [TestClass]
    public class ExportacionPruebas
    {
        private GestorExportacionAuditoria _gestorExportacion;
        private GestorAuditoria _gestorAuditoria;

        [TestInitialize]
        public void Setup()
        {
            _gestorAuditoria = new GestorAuditoria();

            _gestorAuditoria.RegistrarLog(new EntradaDeLog
            {
                Timestamp = new DateTime(2025, 10, 25, 10, 0, 0),
                Usuario = "admin@test.com",
                Accion = EntradaDeLog.AccionLog.AltaUsuario,
                Detalles = "Creación de usuario admin"
            });

            _gestorAuditoria.RegistrarLog(new EntradaDeLog
            {
                Timestamp = new DateTime(2025, 10, 27, 15, 30, 0),
                Usuario = "user@test.com",
                Accion = EntradaDeLog.AccionLog.EditarItem,
                Detalles = "Modificación de item prueba"
            });

            _gestorExportacion = new GestorExportacionAuditoria(_gestorAuditoria);
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

    }
}
