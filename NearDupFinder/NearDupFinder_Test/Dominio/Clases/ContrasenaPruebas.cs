using System.Reflection;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class ContrasenaPruebas
{
    [TestMethod]
    public void Validar_Correcta_RetornaVerdadero() 
    {
        string valida = "123QWEasdzxc@"; 
        Assert.IsTrue(Contrasena.Validar(valida));
    }
    
    [TestMethod]
    public void Validar_LongitudMenorA8_RetornaFalso() 
    {
        string invalida = "Aa1@aaa"; 
        Assert.IsFalse(Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinMayuscula_RetornaFalso() 
    {
        string invalida = "aa1@aaaa"; 
        Assert.IsFalse(Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinMinuscula_RetornaFalso() 
    { 
        string invalida = "AA1@AAAA"; 
        Assert.IsFalse(Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinDigito_RetornaFalso() 
    { 
        string invalida = "Aa@aaaaa"; 
        Assert.IsFalse(Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinCaracterEspecial_RetornaFalso() 
    { 
        string invalida = "Aa1aaaaa"; 
        Assert.IsFalse(Contrasena.Validar(invalida));
    }
    
    [TestMethod]
    public void Validar_Nulo_RetornaFalso()
    {
        Assert.IsFalse(Contrasena.Validar(null!));
    }
    
    [TestMethod]
    public void Crear_ContrasenaValida_GeneraHashNoVacio() 
    { 
        string valida = "Aa1@aaaa"; 
        Contrasena contrasena = Contrasena.Crear(valida);
        
        Assert.IsNotNull(contrasena);
        string hash = contrasena.ObtenerHash();

        Assert.IsFalse(string.IsNullOrWhiteSpace(hash), "El hash no debería ser nulo ni vacío."); 
        Assert.AreEqual(32, hash!.Length, "MD5 en hex debería tener 32 caracteres.");
    }
    
    [TestMethod]
    public void Crear_Nulo_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Crear(null!));
    }
    
    [TestMethod]
    public void Verificar_ContrasenaCorrecta_DevuelveTrue() 
    { 
        string valida = "Aa1@aaaa"; 
        Contrasena contrasena = Contrasena.Crear(valida);
        
        Assert.IsTrue(contrasena.Verificar(valida));
    }

    [TestMethod] 
    public void Verificar_ContrasenaIncorrecta_DevuelveFalso() 
    { 
        string valida = "Aa1@aaaa"; 
        var contrasena = Contrasena.Crear(valida);
        
        Assert.IsFalse(contrasena.Verificar("Aa1@aaab"));
    }

    [TestMethod] 
    public void Hash_MismoInput_GeneraMismoHash() 
    { 
        string valida = "Aa1@aaaa"; 
        Contrasena c1 = Contrasena.Crear(valida); 
        Contrasena c2 = Contrasena.Crear(valida);
        
        string hash1 = c1.ObtenerHash(); 
        string hash2 = c2.ObtenerHash();
        
        Assert.AreEqual(hash1, hash2, "Mismo input debe producir el mismo hash MD5.");
    }

    [TestMethod]
    public void Hash_InputDistinto_GeneraHashDistinto() 
    { 
        string valida1 = "Aa1@aaaa";
        string valida2 = "Aa1@aaab";

        var c1 = Contrasena.Crear(valida1); 
        var c2 = Contrasena.Crear(valida2);
        
        string hash1 = c1.ObtenerHash(); 
        string hash2 = c2.ObtenerHash();

        Assert.AreNotEqual(hash1, hash2, "Inputs distintos deben producir hashes distintos.");
    }
}