namespace NearDupFinder_Dominio.Clases;

public class ItemDTO
{
    public int Id { get; set; }             
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    
    public bool EstadoDuplicado { get; set; }

 
    public static ItemDTO FromEntity(Item item)
    {
        return new ItemDTO
        {
            Id = item.Id,
            Titulo = item.Titulo,
            Descripcion = item.Descripcion,
            Categoria = item.Categoria,
            Marca = item.Marca,
            Modelo = item.Modelo
        };
    }
}