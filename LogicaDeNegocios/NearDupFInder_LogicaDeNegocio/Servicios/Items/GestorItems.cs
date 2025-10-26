using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Items;

public class GestorItems
{
    private readonly HashSet<int> _idsItemsGlobal;

    public GestorItems(
        HashSet<int> idsItemsGlobal)
    {
        _idsItemsGlobal = idsItemsGlobal;
    }

    public void AsegurarIdUnicoPublic(Item item)
    {
        AsegurarIdUnico(item);
    }

    private void AsegurarIdUnico(Item item)
    {
        int idApropiado = item.Id;
        while (IdExisteEnListaDeIdGlobal(idApropiado))
            idApropiado++;
        item.AjustarId(idApropiado);
        _idsItemsGlobal.Add(idApropiado);
    }

    public bool IdExisteEnListaDeIdGlobal(int id)
    {
        return _idsItemsGlobal.Contains(id);
    }
}