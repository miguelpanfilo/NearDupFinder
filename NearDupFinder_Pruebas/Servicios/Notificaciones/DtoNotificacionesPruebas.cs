using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaNotificacion;

namespace NearDupFinder_Pruebas.Servicios.Notificaciones;

[TestClass]
public class DtoNotificacionesPruebas
{
    [TestMethod]
    public void DatosPublicosNotificacion_FromEntity_MapeaIdMensajeYFechaCorrectamente_OkTest()
    {
        var email = "user@test.com";
        var mensaje = "Mensaje de prueba";

        var notif = new Notificacion(email, mensaje);

        var fechaEsperada = new DateTime(2024, 1, 15, 10, 30, 0);

        typeof(Notificacion)
            .GetProperty(nameof(Notificacion.FechaCreacion))!
            .SetValue(notif, fechaEsperada);

        typeof(Notificacion)
            .GetProperty(nameof(Notificacion.Id))!
            .SetValue(notif, 123);

        var dto = DatosPublicosNotificacion.FromEntity(notif);

        Assert.AreEqual(123, dto.Id);
        Assert.AreEqual(mensaje, dto.Mensaje);
        Assert.AreEqual(fechaEsperada, dto.FechaCreacion);
    }
}