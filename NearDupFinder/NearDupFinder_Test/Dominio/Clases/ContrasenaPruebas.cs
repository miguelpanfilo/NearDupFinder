using System.Reflection;
using NearDupFinder_Dominio.Clases;

namespace NearDupFinder_Test.Dominio.Clases;

[TestClass]
public class ContrasenaPruebas
{
    [TestMethod]
    public void Validar_LongitudMenorA8_LanzaExcepcion() 
    {
        string invalida = "Aa1@aaa"; 
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinMayuscula_LanzaExcepcion() 
    {
        string invalida = "aa1@aaaa"; 
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinMinuscula_LanzaExcepcion() 
    { 
        string invalida = "AA1@AAAA"; 
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinDigito_LanzaExcepcion() 
    { 
        string invalida = "Aa@aaaaa"; 
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Validar(invalida));
    }

    [TestMethod] 
    public void Validar_SinCaracterEspecial_LanzaExcepcion() 
    { 
        string invalida = "Aa1aaaaa"; 
        Assert.ThrowsException<ArgumentException>(() => Contrasena.Validar(invalida));
    }
    
    [TestMethod]
    public void Validar_Nulo_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Contrasena.Validar(null!));
    }
    
    [TestMethod]
    public void Crear_ContrasenaValida_GeneraHashNoVacio() 
    { 
        string valida = "Aa1@aaaa"; 
        Contrasena contrasena = Contrasena.Crear(valida);
        
        Assert.IsNotNull(contrasena);
        string hash = contrasena.GetHash();

        Assert.IsFalse(string.IsNullOrWhiteSpace(hash), "El hash no debería ser nulo ni vacío."); 
        Assert.AreEqual(32, hash!.Length, "MD5 en hex debería tener 32 caracteres.");
    }
    
    [TestMethod]
    public void Crear_Nulo_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Contrasena.Crear(null!));
    }
    
    [TestMethod]
    public void Verificar_ContrasenaCorrecta_DevuelveTrue() 
    { 
        string valida = "Aa1@aaaa"; 
        Contrasena contrasena = Contrasena.Crear(valida);
        
        Assert.IsTrue(contrasena.Verificar(valida));
    }

    [TestMethod] 
    public void Verificar_ContrasenaIncorrecta_DevuelveFalse() 
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
        
        string hash1 = c1.GetHash(); 
        string hash2 = c2.GetHash();
        
        Assert.AreEqual(hash1, hash2, "Mismo input debe producir el mismo hash MD5.");
    }

    [TestMethod]
    public void Hash_InputDistinto_GeneraHashDistinto() 
    { 
        string valida1 = "Aa1@aaaa";
        string valida2 = "Aa1@aaab"; // cambia 1 char

        var c1 = Contrasena.Crear(valida1); 
        var c2 = Contrasena.Crear(valida2);
        
        string hash1 = c1.GetHash(); 
        string hash2 = c2.GetHash();

        Assert.AreNotEqual(hash1, hash2, "Inputs distintos deben producir hashes distintos.");
    }
}