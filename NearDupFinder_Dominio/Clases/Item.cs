using NearDupFinder_Dominio.Excepciones;
namespace NearDupFinder_Dominio.Clases;
public class Item
{
    private static int _siguienteId = 1;
    private string? _titulo { get; set; }
    private string? _descripcion { get; set; }
    private string? _marca { get; set; }
    private string? _modelo { get; set; }
    private string? _categoria { get; set; }
    public bool EstadoDuplicado { get; set; }
    public int Id { get; private set; }
    
    public int Stock { get; set; }
    
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
        EstadoDuplicado = false;
    }
    public string? Titulo
    {
        get => _titulo;
        set
        {
            int tituloLargoInavlido = 120;
            if (string.IsNullOrWhiteSpace(value))
                throw new ExcepcionItem("El Título es obligatorio");
            if (value.Length > tituloLargoInavlido)
                throw new ExcepcionItem("El Título no puede superar 120 caracteres.");
            _titulo = value;
        }
    }
    public string? Descripcion
    {
        get => _descripcion;
        set
        {
            int descripcionLargoInavlido = 400;
            if (string.IsNullOrWhiteSpace(value))
                throw new ExcepcionItem("La Descripción es obligatoria.");
            if (value.Length > descripcionLargoInavlido)
                throw new ExcepcionItem("La descripcion no puede superar 400 caracteres.");
            _descripcion = value;
        }
    }
    public string? Marca
    {
        get => _marca;
        set
        {
            int marcaLargoInavlido = 60;
            if (value != null && value.Length > marcaLargoInavlido)
                throw new ExcepcionItem("La marca no puede superar 60 caracteres.");
            _marca = value;
        }
    }
    public string? Modelo
    {
        get => _modelo;
        set
        {
            int modeloLargoInavlido = 60;
            if (value != null && value.Length > modeloLargoInavlido)
                throw new ExcepcionItem("El modelo no puede superar 60 caracteres.");
            _modelo = value;
        }
    }
    public string? Categoria
    {
        get => _categoria;
        set
        {
            int categoriaLargoInavlido = 40;
            if (value != null && value.Length > categoriaLargoInavlido)
                throw new ExcepcionItem("La categoria no puede superar 40 caracteres.");
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
        int primerIdDeUnItemCreado = 1;
        _siguienteId = primerIdDeUnItemCreado;
    }
    public void EditarTitulo(string? nuevoTitulo) => Titulo = nuevoTitulo;
    public void EditarDescripcion(string? nuevaDescripcion) => Descripcion = nuevaDescripcion;
    public void EditarMarca(string? nuevaMarca) => Marca = nuevaMarca;
    public void EditarModelo(string? nuevoModelo) => Modelo = nuevoModelo;
    public void EditarCategoria(string? nuevaCategoria) => Categoria = nuevaCategoria;
    public void AjustarId(int id)
    {
        int idItemInexistente = 0;
        if (id == idItemInexistente)
            throw new ExcepcionItem("El id no es valido");
        Id = id;
        _siguienteId = id + 1;
    }
    public void ModificarIdEnCasoDeImportacion(int id)
    {
        int idItemInexistente = 0;
        if (id == idItemInexistente)
            throw new ExcepcionItem("El id no es valido");
        _siguienteId--;
        Id = id;
    }
    public static void ResetearContadorIdDesde(int nuevoValor)
    {
        if (nuevoValor > 0)
            _siguienteId = nuevoValor;
    }


}
