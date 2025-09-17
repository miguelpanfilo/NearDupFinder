using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio;

public class Catalogo
{
    public string Titulo { get; set; }
    public string Descripcion { get; set; }

   
    private List<Item> items = new List<Item>();

  
    public IReadOnlyList<Item> Items => items.AsReadOnly();

    public void AgregarItem(Item item)
    {
        if (item == null)
            throw new CatalogoException("No se puede agregar un Item nulo.");
        
        items.Add(item);
    }

    public Item ObtenerItemPorId(int id)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
            {
                return item; 
            }
        }

        return null;

    }

    public void EliminarItem(int itemId)
    {
        Item item = ObtenerItemPorId(itemId);
        items.Remove(item);
    }
}

