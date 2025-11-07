using Microsoft.EntityFrameworkCore;
using NearDupFinder_Interfaces; 

namespace NearDupFinder_Almacenamiento.Repositorios;

public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
{
    protected readonly SqlContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositorioGenerico(SqlContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public T? ObtenerPorId(int id)
    {
        return _dbSet.Find(id);
    }

    public List<T> ObtenerTodos()
    {
        return _dbSet.ToList();
    }

    public void Agregar(T entidad)
    {
        _dbSet.Add(entidad);
    }

    public void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }

    public void GuardarCambios()
    {
        _context.SaveChanges();
    }
}