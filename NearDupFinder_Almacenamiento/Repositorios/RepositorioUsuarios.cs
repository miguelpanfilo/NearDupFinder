using Microsoft.EntityFrameworkCore;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioUsuarios(SqlContext contexto) : IRepositorioUsuarios
    {
        public Usuario? ObtenerUsuarioPorId(int id)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .AsNoTracking()
                .SingleOrDefault(u => u.Id == id);

            if (usuario is not null)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuario;
        }

        public Usuario? ObtenerUsuarioPorEmail(string email)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .AsNoTracking()
                .SingleOrDefault(u => u.Email.Valor == email);

            if (usuario is not null)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuario;
        }
        
        public Usuario? ObtenerUsuarioPorEmailParaEdicion(string email)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .SingleOrDefault(u => u.Email.Valor == email);

            if (usuario is not null)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuario;
        }

        public IReadOnlyList<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .AsNoTracking()
                .ToList();

            foreach (Usuario usuario in usuarios)
            {
                usuario.SincronizarRolesDesdePersistencia();
            }

            return usuarios;
        }

        public int ObtenerIdMaximo()
        {
            int? idMaximo = contexto.Usuarios
                .AsNoTracking()
                .Select(usuario => (int?)usuario.Id)
                .Max();

            return idMaximo ?? 0;        }

        public void Agregar(Usuario usuario)
        {
            usuario.SincronizarRolesParaPersistencia();
            contexto.Usuarios.Add(usuario);
            contexto.SaveChanges();
        }

        public void Actualizar(Usuario usuario)
        {
            usuario.SincronizarRolesParaPersistencia();
            contexto.SaveChanges();
        }

        public void Eliminar(int id)
        {
            Usuario? usuario = contexto.Usuarios
                .Include(u => u.RolesPersistidos)
                .SingleOrDefault(u => u.Id == id);

            contexto.Usuarios.Remove(usuario!);
            contexto.SaveChanges();
        }
    }
}
