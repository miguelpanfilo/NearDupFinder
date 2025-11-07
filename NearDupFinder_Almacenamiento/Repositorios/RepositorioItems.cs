using NearDupFinder_Interfaces;     
using NearDupFinder_Dominio.Clases;  

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioItems : RepositorioGenerico<Item>, IRepositorioItems
    {
        public RepositorioItems(SqlContext context) : base(context)
        {
        }

        // Si algún día querés algo específico de Item, lo ponés acá.
    }
}