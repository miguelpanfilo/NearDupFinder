using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio;

public class Item
{
    public string titulo;
    public string descripcion;
    public string marca;
    public string modelo;
    public string categoria;
    public Catalogo catalogo;
    
    public string Marca {get;set;}
    public string Modelo  {get;set;}
    public string Categoria   {get;set;}
   
 

    public Catalogo Catalogo
    {
        get => catalogo;
        set
        {
            if (value == null)
                throw new ArgumentException("El Item debe tener un Catalogo.");
            catalogo = value;
        }
    }
    public string Titulo
    {
        get => titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("El Título es obligatorio");
            if (value.Length > 120)
                throw new ItemException("El Título no puede superar 120 caracteres.");
            titulo = value;
        }
    }
    public string Descripcion
    {
        get => titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("La Descripción es obligatoria.");
            if (value.Length > 400)
                throw new ItemException("La descripcion no puede superar 400 caracteres."); 
            descripcion = value;
        }
    }

}