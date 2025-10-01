using System.Globalization;

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
            // DateTime valida rangos y reglas de bisiestos.
            var fecha = new DateTime(anio, mes, dia);
            return new Fecha(fecha.Year, fecha.Month, fecha.Day);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentException("No es una fecha válida.", nameof(dia), ex);
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
    
    public bool Igual(Fecha? otraFecha)
    {
        if (otraFecha is null) 
            return false;
        return _anio == otraFecha._anio && _mes  == otraFecha._mes && _dia  == otraFecha._dia;
    }
}