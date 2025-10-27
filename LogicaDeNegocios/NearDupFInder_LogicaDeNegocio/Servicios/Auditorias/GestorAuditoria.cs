using NearDupFinder_Dominio.Clases;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;

public class GestorAuditoria
{
    private readonly List<EntradaDeLog> _auditoria = new();

    private readonly Dictionary<EntradaDeLog.AccionLog, string> _descripcionesAccion = new()
    {
        { EntradaDeLog.AccionLog.AltaUsuario, "Creacion de usuario" },
        { EntradaDeLog.AccionLog.EditarUsuario, "Modificacion de usuario" },
        { EntradaDeLog.AccionLog.AltaItem, "Alta de item" },
        { EntradaDeLog.AccionLog.EliminarItem, "Eliminación de item" },
        { EntradaDeLog.AccionLog.DeteccionDuplicados, "Detección duplicados automatica" },
        { EntradaDeLog.AccionLog.ConfirmarDuplicado, "Confirmación de duplicado" },
        { EntradaDeLog.AccionLog.FusionarCluster, "Fusión de cluster" },
        { EntradaDeLog.AccionLog.DescartarDuplicado, "Descartar duplicado" },
        { EntradaDeLog.AccionLog.EditarItem, "Editar item" },
        { EntradaDeLog.AccionLog.EliminarUser, "Eliminacion de usuario" },
    };

    private string _usuarioActual = "No hay usuario logueado";

    public void AsignarUsuarioActual(string email)
    {
        _usuarioActual = email;
    }

    public void DesasignarUsuario()
    {
        _usuarioActual = "No hay usuario logueado";
    }

    public void RegistrarLog(EntradaDeLog.AccionLog accion, string? detalles)
    {
        var entry = new EntradaDeLog
        {
            Timestamp = DateTime.UtcNow,
            Usuario = _usuarioActual,
            Accion = accion,
            Detalles = $"{_descripcionesAccion[accion]}: {detalles}"
        };
        _auditoria.Add(entry);
    }
    public void RegistrarLogManual(DateTime fecha, string usuario, EntradaDeLog.AccionLog accion, string detalles)
    {
        var log = new EntradaDeLog
        {
            Timestamp = fecha,
            Usuario = usuario,
            Accion = accion,
            Detalles = detalles
        };

        _auditoria.Add(log);
    }

    public IReadOnlyList<EntradaDeLog> ObtenerLogs() => _auditoria.AsReadOnly();
}