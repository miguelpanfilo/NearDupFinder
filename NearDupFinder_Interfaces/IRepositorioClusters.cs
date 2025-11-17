using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioClusters : IRepositorioGenerico<Cluster>
{
    void LimpiarCanonicoPorCatalogo(int idCatalogo);
}