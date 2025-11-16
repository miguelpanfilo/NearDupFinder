namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

public record DatosActualizarItem(
    int IdCatalogo,
    int IdItem,
    string? Titulo = null,
    string? Descripcion = null,
    string? Categoria = null,
    string? Marca = null,
    string? Modelo = null,
    int? Stock = null,
    int? Precio = null,
    string? ImagenBase64 = null
);