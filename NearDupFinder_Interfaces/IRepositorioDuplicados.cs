using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Interfaces;

public interface IRepositorioDuplicados
{
    public IReadOnlyList<ParDuplicado> ObtenerListaDeDuplicados();
    
    public void AgregarDuplicado(ParDuplicado parDuplicado);
    
    public void EliminarDuplicado(ParDuplicado parDuplicado);
}