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
    public string Marca
    {
        get => marca;
        set
        {
            if (value != null && value.Length > 60)
                throw new ItemException("La marca no puede superar 60 caracteres.");
            marca = value;
        }
    }
    public string Modelo
    {
        get => modelo;
        set
        {
            if (value != null && value.Length > 60)
                throw new ItemException("El modelo no puede superar 60 caracteres.");
            modelo = value;
        }
    }

}