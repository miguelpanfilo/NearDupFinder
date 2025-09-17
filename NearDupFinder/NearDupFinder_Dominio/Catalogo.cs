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

}