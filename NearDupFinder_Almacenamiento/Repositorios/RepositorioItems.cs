using Microsoft.EntityFrameworkCore;
using NearDupFinder_Interfaces;     
using NearDupFinder_Dominio.Clases;  

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioItems : RepositorioGenerico<Item>, IRepositorioItems
    {
        private readonly SqlContext _ctx;
        private readonly DbSet<Item> _items;

        public RepositorioItems(SqlContext context) : base(context)
        {
            _ctx   = context;
            _items = context.Set<Item>();
        }
        public void AsignarCluster(int idItem, int? idCluster)
        {
            var item = _items.Find(idItem) 
                       ?? throw new InvalidOperationException($"Item {idItem} inexistente.");

            _items.Attach(item);
            var entry = _ctx.Entry(item);

            entry.Property<int?>("ClusterId").CurrentValue = idCluster;
            entry.Property<int?>("ClusterId").IsModified   = true;
        }

    }
}