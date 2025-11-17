using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Pruebas.Dominio.Clases
{
    [TestClass]
    public class NotificacionPruebas
    {
        [TestMethod]
        public void CrearNotificacion_ValoresCorrectos_OkTest()
        {
            string email = "usuario@correo.com";
            string mensaje = "Stock bajo";

            var inicio = DateTime.Now;
            var noti = new Notificacion(email, mensaje);
            var fin = DateTime.Now;

            Assert.AreEqual(email, noti.EmailUsuario);
            Assert.AreEqual(mensaje, noti.Mensaje);
            Assert.IsTrue(noti.FechaCreacion >= inicio && noti.FechaCreacion <= fin);
            Assert.IsFalse(noti.Leida);
        }

        [TestMethod]
        public void MarcarComoLeida_CambiaFalseATrue_OkTest()
        {
            var noti = new Notificacion("a@b.com", "mensaje");

            Assert.IsFalse(noti.Leida);

            noti.MarcarComoLeida();

            Assert.IsTrue(noti.Leida);
        }

        [TestMethod]
        public void MarcarComoLeida_Idempotente_OkTest()
        {
            var noti = new Notificacion("a@b.com", "mensaje");

            noti.MarcarComoLeida();
            noti.MarcarComoLeida();

            Assert.IsTrue(noti.Leida);
        }
    }
}