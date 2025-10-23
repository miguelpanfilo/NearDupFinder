using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Duplicados;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Items;

public class GestorItems
{
    private readonly GestorCatalogos _gestorCatalogos;
    private readonly ControladorDuplicados _controladorDuplicados;
    private readonly HashSet<int> _idsItemsGlobal;
    private readonly GestorAuditoria _gestorAuditoria;

    public GestorItems(
        GestorCatalogos gestorCatalogos,
        ControladorDuplicados controladorDuplicados,
        GestorAuditoria gestorAuditoria,
        HashSet<int> idsItemsGlobal)
    {
        _gestorCatalogos = gestorCatalogos;
        _controladorDuplicados = controladorDuplicados;
        _gestorAuditoria = gestorAuditoria;
        _idsItemsGlobal = idsItemsGlobal;
    }
    public void AsegurarIdUnicoPublic(Item item)
    {
        AsegurarIdUnico(item);
    }

    public void AgregarIdAGlobal(int id)
    {
        _idsItemsGlobal.Add(id);
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