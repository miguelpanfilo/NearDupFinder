namespace NearDupFinder_Dominio.Clases;

public class EntradaDeLog
{
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public string Usuario { get; init; } = "Desconocido";
    public AccionLog Accion { get; init; }
    public string Detalles { get; init; } = string.Empty;
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




