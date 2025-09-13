namespace NearDupFinder_Dominio;

public class Item
{
    public string titulo;
    public string descripcion;
    public string marca;
    public string modelo;
    public string categoria;
    public Catalogo catalogo;
    
    public string Titulo{get;set;}
    public string Descripcion{get;set;}
    public string Marca{get;set;}
    public string Modelo{get;set;}
    public string Categoria{get;set;}
    
    public Catalogo Catalogo
    {
        get => catalogo;
        set => catalogo = value ?? throw new ArgumentException("El Item debe tener un Catalogo.");
    }
    

}