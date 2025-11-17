using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioNotificaciones : IRepositorioGenerico<Notificacion>
{

    Notificacion? ObtenerPorId(int id);
    
    List<Notificacion> ObtenerPorEmail(string emailUsuario);
    
}