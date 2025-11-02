using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

public class DatosPublicosCatalogo(int id, string titulo, string descripcion)
{
    public readonly int Id = id;
    public readonly string Titulo = titulo;
    public readonly string Descripcion = descripcion;
    
    public static DatosPublicosCatalogo FromEntity(Catalogo catalogo)
    {

        return new DatosPublicosCatalogo(
            catalogo.Id,
            catalogo.Titulo,
            catalogo.Descripcion);
    }
}