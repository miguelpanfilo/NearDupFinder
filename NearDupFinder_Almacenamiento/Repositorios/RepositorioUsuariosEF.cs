using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioUsuariosEf(SqlContext contexto) : IRepositorioUsuarios
    {
        public Usuario? ObtenerUsuarioPorId(int id)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .SingleOrDefault(u => u.Id == id);

            if (usuario is not null)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuario;
        }

        public Usuario? ObtenerUsuarioPorEmail(string emailNormalizado)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .SingleOrDefault(u => u.Email.Valor == emailNormalizado);

            if (usuario is not null)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuario;
        }

        public IReadOnlyList<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = contexto.Usuarios
                .AsNoTracking()
                .ToList();

            return usuarios;
        }

        public bool ExisteEmail(string emailNormalizado)
        {
            bool existe = contexto.Usuarios.Any(u => u.Email.Valor == emailNormalizado);
            return existe;
        }
        
        public void Agregar(Usuario usuario)
        {
            contexto.Usuarios.Add(usuario);
            contexto.SaveChanges();
        }

        public void Actualizar(Usuario usuario)
        {
            contexto.Usuarios.Update(usuario);
            contexto.SaveChanges();
        }

        public void Eliminar(Usuario usuario)
        {
            contexto.Usuarios.Remove(usuario);
            contexto.SaveChanges();
        }
    }
}
