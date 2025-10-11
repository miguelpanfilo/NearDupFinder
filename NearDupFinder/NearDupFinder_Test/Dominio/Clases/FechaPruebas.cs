using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class FechaPruebas
{
    [TestMethod]
    public void Crear_DiaInvalido_LanzaArgumentException()
    {
        int anio = 2000, mes = 12, dia = 0;
        
        Assert.ThrowsException<UsuarioException>(() => Fecha.Crear(anio, mes, dia));
    }
    
    [TestMethod]
    public void Crear_MesInvalido_LanzaUsuarioException()
    {
        int anio = 2000, mes = 13, dia = 1;
        
        Assert.ThrowsException<UsuarioException>(() => Fecha.Crear(anio, mes, dia));
    }
    
    [TestMethod]
    public void Crear_AnioInvalido_LanzaUsuarioException()
    {
        int anio = -200, mes = 13, dia = 1;
        
        Assert.ThrowsException<UsuarioException>(() => Fecha.Crear(anio, mes, dia));
    }
    
    [TestMethod]
    public void Crear_AnioNoBisiesto_29Febrero_LanzaUsuarioException()
    {
        int anio = 2001, mes = 2, dia = 29;
        
        Assert.ThrowsException<UsuarioException>(() => Fecha.Crear(anio, mes, dia));
    }
    
    [TestMethod]
    public void CrearFecha_FechaValida_RetornaInstanciaValida()
    {
        int anio = 2001, mes = 12, dia = 31;
        var fecha = Fecha.Crear(anio, mes, dia);
        
        Assert.AreEqual("12-31-2001", fecha.ToString());
    }

    [TestMethod]
    public void ToString_Formatea_RetornaFormato_MM_dd_yyyy()
    {
        int anio = 2001, mes = 5, dia = 7;
        var fecha = Fecha.Crear(anio, mes, dia);
        
        Assert.AreEqual("05-07-2001", fecha.ToString());
    }

    [TestMethod]
    public void ToDateTime_RetornaInstanciaEsperada()
    {
        int anio = 2001, mes = 5, dia = 7;
        var fecha = Fecha.Crear(anio, mes, dia);
        var esperado = new DateTime(2001, 5, 7);

        Assert.AreEqual(esperado, fecha.ToDateTime());
    }
}
