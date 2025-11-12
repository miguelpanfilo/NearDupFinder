using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorItems;

public record DatosCrearItem(
    int IdCatalogo,
    string Titulo,
    string Descripcion,
    string? Categoria = null,
    string? Marca = null,
    string? Modelo = null,
    int? IdImportado = null
)
{
    public Item ToEntity()
    {
        return new Item
        {
            Titulo = Titulo,
            Descripcion = Descripcion,
            Categoria = Categoria,
            Marca = Marca,
            Modelo = Modelo
        };
    }
}