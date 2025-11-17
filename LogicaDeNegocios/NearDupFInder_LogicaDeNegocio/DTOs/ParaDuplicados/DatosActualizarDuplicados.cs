namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaDuplicados;

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