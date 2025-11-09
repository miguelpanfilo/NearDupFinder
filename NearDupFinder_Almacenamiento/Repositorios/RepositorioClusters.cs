using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioClusters: RepositorioGenerico<Cluster>, IRepositorioClusters
{
    public RepositorioClusters(SqlContext context) : base(context)
    {
    }
    
}