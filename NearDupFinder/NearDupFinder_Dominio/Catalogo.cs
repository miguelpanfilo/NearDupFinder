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
    
    public Item ObtenerItemPorId(int i)
    {
        foreach (Item item in items)
        {
            if (item.Id == i)
            {
                return item; 
            }
        }

        return null; 
    }
}