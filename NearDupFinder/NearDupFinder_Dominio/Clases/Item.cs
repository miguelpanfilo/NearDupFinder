using System.Data;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public class Item
{
    private static int _siguienteId = 1;
    private string _titulo = null!;
    private string _descripcion = null!;
    private string? _marca;
    private string? _modelo;
    private string? _categoria;
    public bool EstadoDuplicado = false;
    public int Id { get; private set; }
    
    public Item()
    {
        Id = _siguienteId++;
    }
    public Item(string titulo, string descripcion, string? marca = null, string? modelo = null, string? categoria = null)
    {
        Titulo = titulo;    
        Descripcion = descripcion; 
        Marca = marca;            
        Modelo = modelo;          
        Categoria = categoria;    
        Id = _siguienteId++;
    }
    public string? Titulo
    {
        get => _titulo;
        set
        {
            int tituloDeLargoMaximo = 120;
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("El Título es obligatorio");
            if (value.Length > tituloDeLargoMaximo)
                throw new ItemException("El Título no puede superar 120 caracteres.");
            _titulo = value;
        }
    }
    public string Descripcion
    {
        get => _descripcion;
        set
        {
            int descripcionDeLargoMaximo = 400;
            if (string.IsNullOrWhiteSpace(value))
                throw new ItemException("La Descripción es obligatoria.");
            if (value.Length > descripcionDeLargoMaximo)
                throw new ItemException("La descripcion no puede superar 400 caracteres.");
            _descripcion = value;
        }
    }
    public string? Marca
    {
        get => _marca;
        set
        {
            int marcaDeLargoMaximo = 60;
            if (value != null && value.Length > marcaDeLargoMaximo)
                throw new ItemException("La marca no puede superar 60 caracteres.");
            _marca = value;
        }
    }
    public string? Modelo
    {
        get => _modelo;
        set
        {
            int modeloDeLargoMaximo = 60;
            if (value != null && value.Length > modeloDeLargoMaximo)
                throw new ItemException("El modelo no puede superar 60 caracteres.");
            _modelo = value;
        }
    }
    public string? Categoria
    {
        get => _categoria;
        set
        {
            int categoriaDeLargoMaximo = 40;
            if (value != null && value.Length > categoriaDeLargoMaximo)
                throw new ItemException("La categoria no puede superar 40 caracteres.");
            _categoria = value;
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Item item)
            return false;
        return Id == item.Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public static void ResetearContadorId()
    {
        int primerIdDeUnItem = 1;
      _siguienteId = primerIdDeUnItem;
    }
    public void EditarTitulo(string? nuevoTitulo) => Titulo = nuevoTitulo;

    public void EditarDescripcion(string nuevaDescripcion) => Descripcion = nuevaDescripcion;

    public void EditarMarca(string nuevaMarca) => Marca = nuevaMarca;

    public void EditarModelo(string nuevoModelo) => Modelo = nuevoModelo;

    public void EditarCategoria(string nuevaCategoria) => Categoria = nuevaCategoria;

    public void ModificarId(int id)
    {
        int  primerIdDeUnItemIncorrecto = 0;
        if (id == primerIdDeUnItemIncorrecto)
            throw new ItemException("El id no es valido");

        Id = id;
    }
}
