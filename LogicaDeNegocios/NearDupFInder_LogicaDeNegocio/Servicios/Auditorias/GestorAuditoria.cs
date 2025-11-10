using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;


public class GestorAuditoria
{
    private readonly IRepositorioAuditorias _repositorio;
    private string _usuarioActual = "No hay usuario logueado";
    private readonly SesionUsuarioActual _sesion;
    private readonly Dictionary<EntradaDeLog.AccionLog, string> _descripcionesAccion = new()
    {
        { EntradaDeLog.AccionLog.AltaUsuario, "Creación de usuario" },
        { EntradaDeLog.AccionLog.EditarUsuario, "Modificación de usuario" },
        { EntradaDeLog.AccionLog.AltaItem, "Alta de ítem" },
        { EntradaDeLog.AccionLog.EliminarItem, "Eliminación de ítem" },
        { EntradaDeLog.AccionLog.DeteccionDuplicados, "Detección de duplicados" },
        { EntradaDeLog.AccionLog.ConfirmarDuplicado, "Confirmación de duplicado" },
        { EntradaDeLog.AccionLog.FusionarCluster, "Fusión de clúster" },
        { EntradaDeLog.AccionLog.DescartarDuplicado, "Descartar duplicado" },
        { EntradaDeLog.AccionLog.EditarItem, "Edición de ítem" },
        { EntradaDeLog.AccionLog.EliminarUser, "Eliminación de usuario" },
    };

    public GestorAuditoria(IRepositorioAuditorias repositorio, SesionUsuarioActual sesion)
    {
        _repositorio = repositorio;
        _sesion = sesion;
    }
    
    public void RegistrarLog(EntradaDeLog.AccionLog accion, string? detalles, string? email = null)
    {
        var usuario = email ?? _sesion.EmailActual ?? "No hay usuario logueado";
        var entrada = new EntradaDeLog
        {
            Timestamp = DateTime.UtcNow,
            Usuario = usuario,
            Accion = accion,
            Detalles = $"{_descripcionesAccion[accion]}: {detalles}"
        };

        _repositorio.Agregar(entrada);
        _repositorio.GuardarCambios();
    }


    public void RegistrarLogManual(DateTime fecha, string usuario, EntradaDeLog.AccionLog accion, string detalles)
    {
        var entrada = new EntradaDeLog
        {
            Timestamp = fecha,
            Usuario = usuario,
            Accion = accion,
            Detalles = detalles
        };

        _repositorio.Agregar(entrada);
        _repositorio.GuardarCambios();
    }

    public IReadOnlyList<EntradaDeLog> ObtenerTodos()
    {
        return _repositorio.ObtenerTodos();
    }

    public IReadOnlyList<EntradaDeLog> ObtenerPorUsuario(string email)
    {
        return _repositorio.ObtenerPorUsuario(email);
    }

    public IReadOnlyList<EntradaDeLog> ObtenerPorRangoDeFechas(DateTime inicio, DateTime fin)
    {
        return _repositorio.ObtenerPorRangoDeFechas(inicio, fin);
    }
    public void AsignarUsuarioActual(string email) => _sesion.Asignar(email);
    public void DesasignarUsuario() => _sesion.Desasignar();
}