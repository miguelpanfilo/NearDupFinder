namespace NearDupFinder_Interfaces;

    public interface IRepositorioGenerico<T> where T : class
    {
        T? ObtenerPorId(int id);
        List<T> ObtenerTodos();
        void Agregar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
        void GuardarCambios();
    }
