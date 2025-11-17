using NearDupFinder_Dominio.Clases;
using Microsoft.EntityFrameworkCore;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioNotificaciones :RepositorioGenerico<Notificacion>, IRepositorioNotificaciones
{
    public RepositorioNotificaciones(SqlContext context) : base(context)
    {
    }

    public new Notificacion? ObtenerPorId(int id) =>
        _context.Notificaciones.FirstOrDefault(n => n.Id == id);

    public List<Notificacion> ObtenerPorEmail(string emailUsuario)
    {
        return _context.Notificaciones
            .Where(n => n.EmailUsuario == emailUsuario)
            .ToList();
    }
}