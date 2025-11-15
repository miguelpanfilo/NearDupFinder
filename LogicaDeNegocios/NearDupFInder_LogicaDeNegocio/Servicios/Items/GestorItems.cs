using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

namespace NearDupFinder_LogicaDeNegocio.Servicios.Items;

public class GestorItems
{
    private readonly IRepositorioItems _repositorioItems;

    public GestorItems(IRepositorioItems repositorioItems)
    {
        _repositorioItems = repositorioItems;
    }

    public void AsegurarIdUnico(Item item)
    {
        int idApropiado = item.Id;
        while (ExisteItemConEseId(idApropiado))
            idApropiado++;
        
        item.AjustarId(idApropiado);
    }
    public void AgregarItemACatalogo(Catalogo catalogo, Item item)
    {
        catalogo.AgregarItem(item);
        GuardarItem(item);
    }

    public bool ExisteItemConEseId(int id)
    {
        bool existe = _repositorioItems.ObtenerPorId(id) != null;
        return existe;
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
    public Item CrearNuevoItem(DatosCrearItem datos)
    {
        return Item.Crear(datos.Titulo, datos.Descripcion, datos.Categoria, 
            datos.Marca, datos.Modelo, datos.Stock);
    }
}