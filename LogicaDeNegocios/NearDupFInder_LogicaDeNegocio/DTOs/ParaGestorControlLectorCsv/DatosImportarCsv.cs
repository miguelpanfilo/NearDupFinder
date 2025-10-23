using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Importacion;

namespace NearDupFinder_LogicaDeNegocio.DTOs.ParaGestorControlLectorCsv;

public class DatosImportarCsv
{
    public List<string> Titulos { get; }
    public int Cantidad { get; }
    public List<Fila> Filas { get; }

    public DatosImportarCsv(List<string> titulos, int cantidad, List<Fila> filas)
    {
        Titulos = titulos;
        Cantidad = cantidad;
        Filas = filas;
    }
}