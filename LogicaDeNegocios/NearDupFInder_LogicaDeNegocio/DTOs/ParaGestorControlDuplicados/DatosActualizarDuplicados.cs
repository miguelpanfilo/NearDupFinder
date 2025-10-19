namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlDuplicados;

public class DatosActualizarDuplicados
{
    public int IdCatalogo { get; }
    public int IdItem { get; }

    public DatosActualizarDuplicados(int idCatalogo, int idItem)
    {
        IdCatalogo = idCatalogo;
        IdItem = idItem;
    }
}