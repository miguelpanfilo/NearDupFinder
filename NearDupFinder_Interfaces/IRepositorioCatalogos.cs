using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioCatalogos : IRepositorioGenerico<Catalogo>
{
    Catalogo? ObtenerPorTitulo(string titulo);
}
