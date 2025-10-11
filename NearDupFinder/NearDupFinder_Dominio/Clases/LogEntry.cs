namespace NearDupFinder_Dominio.Clases;

public class LogEntry
{
    public DateTime Timestamp { get; init; } = DateTime.Now; // Local
    public string Usuario { get; init; } = "Desconocido";
    public AccionLog Accion { get; init; }
    public string Detalles { get; init; } = string.Empty;

    public override string ToString()
    {
        string timestampIso = Timestamp.ToString("yyyy-MM-ddTHH:mm:ss");
        return $"{timestampIso} | {Usuario} | {Accion} | {Detalles}";
    }
    public enum AccionLog
    {
        AltaItem,
        EditarItem,
        EliminarItem,
        DescartarDuplicado,
        ConfirmarDuplicado,
        FusionarCluster,
        AltaUsuario,
        EditarUsuario,
        DeteccionDuplicados,
        EliminarUser
    
    }

    
}




