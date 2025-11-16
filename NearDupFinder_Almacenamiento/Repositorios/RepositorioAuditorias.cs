using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioAuditorias : RepositorioGenerico<EntradaDeLog>, IRepositorioAuditorias
    {
        public RepositorioAuditorias(SqlContext context) : base(context) { }

        public List<EntradaDeLog> ObtenerPorUsuario(string email)
        {
            return _dbSet.Where(a => a.Usuario == email).ToList();
        }

        public List<EntradaDeLog> ObtenerPorRangoDeFechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            return _dbSet
                .Where(a => a.Timestamp >= fechaInicio && a.Timestamp <= fechaFinal)
                .OrderBy(a => a.Timestamp)
                .ToList();
        }
    }
}