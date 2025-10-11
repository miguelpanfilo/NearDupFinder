using System.Globalization;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Fecha
{
    private readonly int _anio;
    private readonly int _mes;
    private readonly int _dia;
    
    private Fecha(int anio, int mes, int dia)
    {
        _anio = anio;
        _mes  = mes;
        _dia  = dia;
    }
    
    public static Fecha Crear(int anio, int mes, int dia)
    {
        try
        {
            var fecha = new DateTime(anio, mes, dia);
            return new Fecha(fecha.Year, fecha.Month, fecha.Day);
        }
        catch
        {
            throw new UsuarioException("No es una fecha válida.");
        }
    }

    public override string ToString()
    {
        return new DateTime(_anio, _mes, _dia).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
    }

    public DateTime ToDateTime()
    {
        return new DateTime(_anio, _mes, _dia);
    }
}