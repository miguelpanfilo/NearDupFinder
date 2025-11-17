using NearDupFInder_LogicaDeNegocio.DTOs.ParaGestorAuditoria;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;

public class ControladorAuditoria
{
    private readonly GestorAuditoria _gestor;

    public ControladorAuditoria(GestorAuditoria gestor)
    {
        _gestor = gestor;
    }

    public IReadOnlyList<AuditoriaDto> ObtenerTodos()
    {
        return _gestor.ObtenerTodasLasEntidades()
            .Select(_gestor.MapeoADto)
            .OrderBy(x => x.Timestamp)
            .ToList();
    }
}