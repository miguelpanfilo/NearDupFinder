namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

public class DatosEliminarItem
{
    public int IdCatalogo { get; }
    public int IdItem { get; }

    public DatosEliminarItem(int idCatalogo, int idItem)
    {
        IdCatalogo = idCatalogo;
        IdItem = idItem;
    }
}