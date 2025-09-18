using NearDupFinder_Dominio.Usuario;

namespace NearDupFinder_Test.Usuario.Dominio.VO;

[TestClass]
public class FechaPruebas
{
    [DataTestMethod]
    [DataRow(2001, 12, 31, "12-31-2001")]
    [DataRow(1995,  1,  1, "01-01-1995")]
    [DataRow(2024,  2, 29, "02-29-2024")] // año bisiesto
    public void Crear_Valido_DevuelveInstancia(int anio, int mes, int dia, string esperado)
    {
        var fecha = Fecha.Crear(anio, mes, dia);
        Assert.AreEqual(esperado, fecha.ToString());
    }

    [DataTestMethod]
    [DataRow(2001,  2, 29)] // 2001 no es bisiesto
    [DataRow(2000, 13,  1)] // mes inválido
    [DataRow(2000,  0, 10)] // mes 0
    [DataRow(2000, 12,  0)] // día 0
    [DataRow(2000,  4, 31)] // abril 31
    public void Crear_Invalido_LanzaExcepcion(int anio, int mes, int dia)
    {
        Assert.ThrowsException<ArgumentException>(() => Fecha.Crear(anio, mes, dia));
    }

    [TestMethod]
    public void ToString_FormateaComoMM_DD_YYYY()
    {
        var fecha = Fecha.Crear(2001, 5, 7); // 7 de mayo de 2001
        Assert.AreEqual("05-07-2001", fecha.ToString());
    }
    
    [TestMethod]
    public void Igual_Valido()
    {
        var fecha1 = Fecha.Crear(2000, 2, 1);
        var fecha2 = Fecha.Crear(2000, 2, 1);

        Assert.IsTrue(fecha1.Igual(fecha2));
    }

    [TestMethod]
    public void Igual_Invalido()
    {
        var fecha1 = Fecha.Crear(2000, 12, 31);
        var fecha2 = Fecha.Crear(2000, 12, 30);

        Assert.IsFalse(fecha1.Igual(fecha2));
    }
    
    [TestMethod]
    public void Igual_Nulo_DevuelveFalse()
    {
        var fecha = Fecha.Crear(2000, 12, 31);

        Assert.IsFalse(fecha.Igual(null));
    }
}