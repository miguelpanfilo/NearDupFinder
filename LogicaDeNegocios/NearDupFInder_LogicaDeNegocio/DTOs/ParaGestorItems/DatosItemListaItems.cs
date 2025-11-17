using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

public class DatosItemListaItems
{
    public int Id { get; set; }             
    public string? Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public string? Categoria { get; set; } = string.Empty;
    public string? Marca { get; set; } = string.Empty;
    public string? Modelo { get; set; } = string.Empty;

    public int Stock { get; set; }
    public int Precio { get; set; }
    public string? ImagenBase64 { get; set; }

    public bool EstadoDuplicado { get; set; }
    
    public static DatosItemListaItems FromEntity(Item item)
    {
        return new DatosItemListaItems
        {
            Id = item.Id,
            Titulo = item.Titulo,
            Descripcion = item.Descripcion,
            Categoria = item.Categoria ?? string.Empty,
            Marca = item.Marca ?? string.Empty,
            Modelo = item.Modelo ?? string.Empty,
            Stock = item.Stock,
            Precio = item.Precio,
            ImagenBase64 = item.ImagenBase64,
            EstadoDuplicado = item.EstadoDuplicado
        };
    }
}