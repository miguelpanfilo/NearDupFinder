using Moq;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.Servicios.Notificaciones;

namespace NearDupFinder_Pruebas.Servicios.Notificaciones
{
    [TestClass]
    public class GestorNotificacionesPruebas
    {
        private const string EmailUsuario = "usuario@prueba.com";

        private static Item CrearItem(string titulo, string descripcion)
        {
            return Item.Crear(titulo, descripcion);
        }

        private static Cluster CrearClusterConCanonicoYEmail(
            string tituloCanonico,
            string descripcionCanonico,
            string emailRevisor,
            int umbral = 10)
        {
            var itemCanonico = CrearItem(tituloCanonico, descripcionCanonico);
            var cluster = new Cluster(1, new HashSet<Item> { itemCanonico });
            cluster.FusionarCanonico(emailRevisor);
            
            cluster.StockMinimoCanonico = umbral;
            cluster.EmailRevisorCreador = emailRevisor;

            return cluster;
        }

        [TestMethod]
        public void NotificarStockBajo_SinCanonico_NoAgregaNiGuarda_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var clusterSinCanonico = new Cluster(1, new HashSet<Item>());
            clusterSinCanonico.EmailRevisorCreador = EmailUsuario;
            clusterSinCanonico.StockMinimoCanonico = 5;

            gestor.NotificarStockBajo(clusterSinCanonico);

            repoMock.Verify(r => r.Agregar(It.IsAny<Notificacion>()), Times.Never);
            repoMock.Verify(r => r.GuardarCambios(), Times.Never);
        }

        [TestMethod]
        public void NotificarStockBajo_SinEmailRevisor_NoAgregaNiGuarda_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var itemCanonico = CrearItem("Titulo", "Desc");
            var cluster = new Cluster(1, new HashSet<Item> { itemCanonico });
            cluster.FusionarCanonico(EmailUsuario);
            cluster.EmailRevisorCreador = string.Empty;
            cluster.StockMinimoCanonico = 10;

            gestor.NotificarStockBajo(cluster);

            repoMock.Verify(r => r.Agregar(It.IsAny<Notificacion>()), Times.Never);
            repoMock.Verify(r => r.GuardarCambios(), Times.Never);
        }

        [TestMethod]
        public void NotificarStockBajo_ConCanonicoYEmail_CreaNotificacionYGuarda_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var cluster = CrearClusterConCanonicoYEmail("Canonico", "Desc", EmailUsuario, umbral: 10);

            Notificacion? notificacionCreada = null;
            repoMock.Setup(r => r.Agregar(It.IsAny<Notificacion>()))
                .Callback<Notificacion>(n => notificacionCreada = n);

            gestor.NotificarStockBajo(cluster);

            Assert.IsNotNull(notificacionCreada);
            Assert.AreEqual(EmailUsuario, notificacionCreada!.EmailUsuario);
            StringAssert.Contains(notificacionCreada.Mensaje, "Canonico");
            StringAssert.Contains(notificacionCreada.Mensaje, "0/10");

            repoMock.Verify(r => r.Agregar(It.IsAny<Notificacion>()), Times.Once);
            repoMock.Verify(r => r.GuardarCambios(), Times.Once);
        }

        [TestMethod]
        public void ObtenerNoLeidas_FiltraPorEmailYLeidasYOrdenaPorFecha_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var ahora = DateTime.Now;
            var n1 = CrearNotificacionConFecha(EmailUsuario, "msg1", ahora.AddMinutes(-10), leida: false);
            var n2 = CrearNotificacionConFecha(EmailUsuario, "msg2", ahora.AddMinutes(-5), leida: true);
            var n3 = CrearNotificacionConFecha("otro@user.com", "msg3", ahora.AddMinutes(-20), leida: false);
            var n4 = CrearNotificacionConFecha(EmailUsuario, "msg4", ahora.AddMinutes(-1), leida: false);

            repoMock.Setup(r => r.ObtenerTodos())
                .Returns(new List<Notificacion> { n1, n2, n3, n4 });

            var resultado = gestor.ObtenerNoLeidas(EmailUsuario).ToList();

            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("msg1", resultado[0].Mensaje);
            Assert.AreEqual("msg4", resultado[1].Mensaje);
        }

        [TestMethod]
        public void MarcarComoLeida_NotificacionExistente_LaMarcaLeidaLaActualizaYLaElimina_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var notif = new Notificacion(EmailUsuario, "mensaje");
            var idNotif = 10;

            repoMock.Setup(r => r.ObtenerPorId(idNotif)).Returns(notif);

            gestor.MarcarComoLeida(idNotif);

            Assert.IsTrue(notif.Leida);
            repoMock.Verify(r => r.Actualizar(notif), Times.Once);
            repoMock.Verify(r => r.Eliminar(notif), Times.Once);
            repoMock.Verify(r => r.GuardarCambios(), Times.Exactly(2));
        }

        [TestMethod]
        public void MarcarComoLeida_NotificacionInexistente_LanzaInvalidOperationException_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            repoMock.Setup(r => r.ObtenerPorId(It.IsAny<int>())).Returns((Notificacion?)null);

            Assert.ThrowsException<InvalidOperationException>(() => gestor.MarcarComoLeida(1));

            repoMock.Verify(r => r.Actualizar(It.IsAny<Notificacion>()), Times.Never);
            repoMock.Verify(r => r.Eliminar(It.IsAny<Notificacion>()), Times.Never);
            repoMock.Verify(r => r.GuardarCambios(), Times.Never);
        }

        [TestMethod]
        public void EliminarNotificacionLeida_NoLeida_NoHaceNada_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var notif = new Notificacion(EmailUsuario, "mensaje");

            gestor.EliminarNotificacionLeida(notif);

            repoMock.Verify(r => r.Eliminar(It.IsAny<Notificacion>()), Times.Never);
            repoMock.Verify(r => r.GuardarCambios(), Times.Never);
        }

        [TestMethod]
        public void EliminarNotificacionLeida_Leida_EliminaYGuarda_OkTest()
        {
            var repoMock = new Mock<IRepositorioNotificaciones>();
            var gestor = new GestorNotificaciones(repoMock.Object);

            var notif = new Notificacion(EmailUsuario, "mensaje");
            notif.MarcarComoLeida();

            gestor.EliminarNotificacionLeida(notif);

            repoMock.Verify(r => r.Eliminar(notif), Times.Once);
            repoMock.Verify(r => r.GuardarCambios(), Times.Once);
        }

        private static Notificacion CrearNotificacionConFecha(string email, string mensaje, DateTime fecha, bool leida)
        {
            var notif = new Notificacion(email, mensaje);

            typeof(Notificacion)
                .GetProperty(nameof(Notificacion.FechaCreacion))!
                .SetValue(notif, fecha);

            if (leida)
                notif.MarcarComoLeida();

            return notif;
        }
    }
}
