using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Items;

public class GestorItems
{
    private readonly HashSet<int> _idsItemsGlobal;
    private readonly IRepositorioItems _repositorioItems;

    public GestorItems(HashSet<int> idsItemsGlobal, IRepositorioItems repositorioItems)
    {
        _idsItemsGlobal = idsItemsGlobal;
        _repositorioItems = repositorioItems;
    }

    public void AsegurarIdUnicoPublico(Item item)
    {
        int idApropiado = item.Id;
        while (IdExisteEnListaDeIdGlobal(idApropiado))
            idApropiado++;
        item.AjustarId(idApropiado);
        _idsItemsGlobal.Add(idApropiado);
    }
    public void AgregarItemACatalogo(Catalogo catalogo, Item item)
    {
        catalogo.AgregarItem(item);
        GuardarItem(item);
    }

    public bool IdExisteEnListaDeIdGlobal(int id)
    {
        return _idsItemsGlobal.Contains(id);
    }

    public void GuardarItem(Item item)
    {
        _repositorioItems.Agregar(item);
        _repositorioItems.GuardarCambios();
    }

    public void ActualizarItem(Item item)
    {
        _repositorioItems.Actualizar(item);
        _repositorioItems.GuardarCambios();
    }

    public void EliminarItem(Item item)
    {
        Item? itemPersistido = _repositorioItems.ObtenerPorId(item.Id);
        if (itemPersistido is null) 
            return;

        _repositorioItems.Eliminar(itemPersistido);
        _repositorioItems.GuardarCambios();
    }
    public Item CrearEntidad(DatosCrearItem datos)
    {
        return datos.ToEntity();
    }


}