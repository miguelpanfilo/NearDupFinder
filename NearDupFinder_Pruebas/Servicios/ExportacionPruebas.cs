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



    }
}
