namespace NearDupFinder_Dominio.Clases;

public record LogEntry
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
}
