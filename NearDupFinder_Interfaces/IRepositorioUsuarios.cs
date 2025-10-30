using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioUsuarios
{
    Usuario? ObtenerUsuarioPorId(int id);
    Usuario? ObtenerUsuarioPorEmail(string email);
    
    void Agregar(Usuario usuario);
    void Actualizar(Usuario usuario);
    void Eliminar(Usuario usuario);

    IReadOnlyList<Usuario> ListarUsuarios(); 
}