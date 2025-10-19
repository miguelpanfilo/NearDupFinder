namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

public class DatosCatalogoEditar
{
    public int Id { get;}
    public string? Titulo { get;}
    public string? Descripcion { get;}

    public DatosCatalogoEditar(int id, string titulo, string descripcion)
    {
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
    }
}