using NearDupFinder_Dominio.Clases;

namespace NearDupFInder_LogicaDeNegocio.DTOs.ParaNotificacion;

public class DatosPublicosNotificacion(int id,
    string mensaje,
    DateTime fechaCreacion)
{
    public int Id { get; } = id;
    public string Mensaje { get; } = mensaje;
    public DateTime FechaCreacion { get; }= fechaCreacion;
    
    public static DatosPublicosNotificacion FromEntity(Notificacion notificacion)
    {
        return new DatosPublicosNotificacion(
            notificacion.Id,
            notificacion.Mensaje,
            notificacion.FechaCreacion
        );
    }
}