using System.Globalization;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Fecha
{
    private Fecha() {}

    public int Anio { get; private set; }
    public int Mes  { get; private set; }
    public int Dia  { get; private set; }
    
    public Fecha(int anio, int mes, int dia)
    {
        Anio = anio;
        Mes  = mes;
        Dia  = dia;
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
            throw new ExcepcionDeUsuario("No es una fecha válida.");
        }
    }

    public override string ToString()
    {
        return new DateTime(Anio, Mes, Dia).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
    }

    public DateTime ToDateTime()
    {
        return new DateTime(Anio, Mes, Dia);
    }
}