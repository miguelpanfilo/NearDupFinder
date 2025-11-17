using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NearDupFinder_Almacenamiento.Repositorios
{
    public class RepositorioAuditorias : RepositorioGenerico<EntradaDeLog>, IRepositorioAuditorias
    {
        public RepositorioAuditorias(SqlContext context) : base(context) { }

      
    }
}