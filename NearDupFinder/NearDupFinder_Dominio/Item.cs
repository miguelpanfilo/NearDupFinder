using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio;

public class Item
{
    private static int _nextId = 1;
    private string _titulo;
    private string _descripcion;
    private string _marca;
    private string _modelo;
    private string _categoria;
    private Catalogo _catalogo;
    public int Id { get; private set; }

    public Item()
    {
        Id = _nextId++;
    }

    public Catalogo Catalogo
    {
        get => _catalogo;
        set
        {
            if (value == null)
                throw new ItemException("El Item debe tener un Catalogo.");
            _catalogo = value;
        }
    }

    public string Titulo
    {
        get => _titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("El Título es obligatorio");
            if (value.Length > 120)
                throw new ItemException("El Título no puede superar 120 caracteres.");
            _titulo = value;
        }
    }

    public string Descripcion
    {
        get => _descripcion;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("La Descripción es obligatoria.");
            if (value.Length > 400)
                throw new ItemException("La descripcion no puede superar 400 caracteres.");
            _descripcion = value;
        }
    }

    public string Marca
    {
        get => _marca;
        set
        {
            if (value != null && value.Length > 60)
                throw new ItemException("La marca no puede superar 60 caracteres.");
            _marca = value;
        }
    }

    public string Modelo
    {
        get => _modelo;
        set
        {
            if (value != null && value.Length > 60)
                throw new ItemException("El modelo no puede superar 60 caracteres.");
            _modelo = value;
        }
    }

    public string Categoria
    {
        get => _categoria;
        set
        {
            if (value != null && value.Length > 40)
                throw new ItemException("La categoria no puede superar 40 caracteres.");
            _categoria = value;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not Item other)
            return false;
        return this.Id == other.Id;
    }

    public static void ResetIdCounter()
    {
      _nextId = 1;
    }
}
