using System.Globalization;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Dominio.Clases;

public sealed class Fecha
{
    private static int _siguienteId = 1;

    private int Id { get; set; }
    
    private int Anio { get; set; }
    private int Mes { get; set; }
    private int Dia { get; set; }
    
    public Fecha(int anio, int mes, int dia)
    {
        Anio = anio;
        Mes  = mes;
        Dia  = dia;
        Id = _siguienteId++;
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