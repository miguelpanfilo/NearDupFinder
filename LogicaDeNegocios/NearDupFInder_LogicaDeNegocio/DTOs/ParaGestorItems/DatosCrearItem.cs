
namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

public record DatosCrearItem(
    int IdCatalogo,
    string Titulo,
    string Descripcion,
    string? Categoria = null,
    string? Marca = null,
    string? Modelo = null,
    int? IdImportado = null,
    int? Stock = null,
    int? Precio = null,
    string? ImagenBase64 = null
);