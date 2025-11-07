using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioUsuarios
{
    Usuario? ObtenerUsuarioPorId(int id);
    Usuario? ObtenerUsuarioPorEmail(string email);
    
    Usuario? ObtenerUsuarioPorEmailParaEdicion(string email);
    
    void Agregar(Usuario usuario);
    void Actualizar(Usuario usuario);
    void Eliminar(int id);
    
    IReadOnlyList<Usuario> ListarUsuarios();

    int ObtenerIdMaximo();
}