using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio;

public class Catalogo
{
    public string titulo;
    public string descripcion;
    public List<Item> items = new List<Item>();
    public string Titulo{get;set;}
    public string Descripcion{get;set;}
    
    public void AgregarItem(Item item)
    {
        
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

        throw new CatalogoException($"No existe ningún Item con Id {id} en este catálogo.");
    }

    public void EliminarItem(int item1Id)
    {
        Item item= ObtenerItemPorId(item1Id);
        
        items.Remove(item);
    }
}