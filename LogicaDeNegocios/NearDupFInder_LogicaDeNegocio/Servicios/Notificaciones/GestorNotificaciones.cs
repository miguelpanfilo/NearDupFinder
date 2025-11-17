using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.DTOs.ParaNotificacion;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Notificaciones;

public class GestorNotificaciones
{
    private readonly IRepositorioNotificaciones _repo;

    public GestorNotificaciones(IRepositorioNotificaciones repo)
    {
        _repo = repo;
    }

    public void NotificarStockBajo(Cluster cluster)
    {
        if (cluster.Canonico == null || string.IsNullOrEmpty(cluster.EmailRevisorCreador))
            return;

        string mensaje =
            $"El stock del ítem canónico '{cluster.Canonico.Titulo}' " +
            $"bajó por debajo del umbral definido ({cluster.StockActual}/{cluster.UmbralStock}).";

        var notificacion = new Notificacion(cluster.EmailRevisorCreador, mensaje);

        _repo.Agregar(notificacion);
        _repo.GuardarCambios();
    }
    
    public IReadOnlyCollection<DatosPublicosNotificacion> ObtenerNoLeidas(string emailUsuario)
    {
        var notificaciones = _repo.ObtenerTodos()
            .Where(n => n.EmailUsuario == emailUsuario && !n.Leida)
            .OrderBy(n => n.FechaCreacion)
            .ToList();;
        return notificaciones.Select(DatosPublicosNotificacion.FromEntity).ToList();
    }

    public void MarcarComoLeida(int idNotificacion)
    {
        var notif = _repo.ObtenerPorId(idNotificacion)
                    ?? throw new InvalidOperationException("Notificación inexistente.");

        notif.MarcarComoLeida();
        _repo.Actualizar(notif);
        _repo.GuardarCambios();
        EliminarNotificacionLeida(notif);
    }

    public void EliminarNotificacionLeida(Notificacion notificacion)
    {
        if (notificacion.Leida)
        {
            _repo.Eliminar(notificacion);
            _repo.GuardarCambios();
        }
    }
    
}