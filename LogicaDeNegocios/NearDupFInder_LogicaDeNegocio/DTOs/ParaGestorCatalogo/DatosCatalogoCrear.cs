using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorCatalogo;

public class DatosCatalogoCrear
{
    public string Titulo { get;}
    public string? Descripcion { get;}
    
    public DatosCatalogoCrear(string titulo, string? descripcion = null )
    {
        Titulo = titulo;
        Descripcion = descripcion;
    }
    
}