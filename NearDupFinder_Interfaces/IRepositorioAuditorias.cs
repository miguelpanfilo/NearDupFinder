using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces
{
    public interface IRepositorioAuditorias : IRepositorioGenerico<EntradaDeLog>
    {
        List<EntradaDeLog> ObtenerPorUsuario(string email);
        List<EntradaDeLog> ObtenerPorRangoDeFechas(DateTime fechaInicio, DateTime fechaFinal);
    }
}