using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioCatalogos : IRepositorioGenerico<Catalogo>
{
    Catalogo? ObtenerPorTitulo(string titulo);
    
    Catalogo? ObtenerParaEliminacionPorId(int idCatalogo);
    
    void LimpiarSeguimiento();
}
