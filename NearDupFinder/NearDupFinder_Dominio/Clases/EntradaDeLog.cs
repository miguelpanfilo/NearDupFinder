using System.Diagnostics.CodeAnalysis;

namespace NearDupFinder_Dominio.Clases;

public class EntradaDeLog
{
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public string Usuario { get; init; } = "Desconocido";
    public AccionLog Accion { get; init; }
    public string Detalles { get; init; } = string.Empty;

    [ExcludeFromCodeCoverage]
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




