using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioItems : IRepositorioGenerico<Item>
{
        void AsignarCluster(int idItem, int? idCluster);
}
