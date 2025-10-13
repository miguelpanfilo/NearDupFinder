using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Pruebas.Dominio.Clases;

[TestClass]
public class ClavePruebas
{
    [TestMethod]
    public void Validar_ClaveNula_RetornaFalso()
    {
        string? clave = null;
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }
    
    [TestMethod]
    public void Validar_ClaveConLongitudMenorA8_RetornaFalso()
    {
        string clave = "Aa1@aaa";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }

    [TestMethod]
    public void Validar_ClaveSinMayuscula_RetornaFalso()
    {
        string clave = "aa1@aaaa";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }

    [TestMethod]
    public void Validar_ClaveSinMinuscula_RetornaFalso()
    {
        string clave = "AA1@AAAA";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }

    [TestMethod]
    public void Validar_ClaveSinDigito_RetornaFalso()
    {
        string clave = "Aa@aaaaa";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }

    [TestMethod]
    public void Validar_ClaveSinCaracterEspecial_RetornaFalso()
    {
        string clave = "Aa1aaaaa";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsFalse(esValida);
    }
    
    [TestMethod]
    public void Validar_ClaveCorrecta_RetornaVerdadero()
    {
        string clave = "123QWEasdzxc@";
        
        bool esValida = Clave.Validar(clave);
        
        Assert.IsTrue(esValida);
    }
    
    [TestMethod]
    public void CrearClave_ClaveNula_LanzaUsuarioException()
    {
        string? claveTexto = null;
        
        Assert.ThrowsException<ExcepcionDeUsuario>(() => Clave.Crear(claveTexto));
    }
    
    [TestMethod]
    public void CrearClave_ClaveValida_GeneraHashNoVacio()
    {
        string claveTexto = "Aa1@aaaa";
        Clave clave = Clave.Crear(claveTexto);
        
        string hash = clave.ObtenerHash();

        Assert.IsFalse(string.IsNullOrWhiteSpace(hash));
    }
    
    [TestMethod]
    public void CrearClave_MismaClave_GeneraMismoHash()
    {
        string claveTexto = "Aa1@aaaa";
        Clave c1 = Clave.Crear(claveTexto);
        Clave c2 = Clave.Crear(claveTexto);
        
        string hash1 = c1.ObtenerHash();
        string hash2 = c2.ObtenerHash();

        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void CrearClave_DistintaClave_GeneraHashDistinto()
    {
        string claveTexto1 = "Aa1@aaaa";
        string claveTexto2 = "Aa1@aaab";
        Clave c1 = Clave.Crear(claveTexto1);
        Clave c2 = Clave.Crear(claveTexto2);

        string hash1 = c1.ObtenerHash();
        string hash2 = c2.ObtenerHash();

        Assert.AreNotEqual(hash1, hash2);
    }
    
    [TestMethod]
    public void Verificar_ClaveCorrecta_DevuelveTrue()
    {
        string claveTexto = "Aa1@aaaa";
        Clave clave = Clave.Crear(claveTexto);

        bool claveEsCorrecta = clave.Verificar(claveTexto);

        Assert.IsTrue(claveEsCorrecta);
    }

    [TestMethod]
    public void Verificar_ClaveIncorrecta_DevuelveFalso()
    {
        string claveTexto = "Aa1@aaaa";
        Clave clave = Clave.Crear(claveTexto);

        bool claveEsCorrecta = clave.Verificar("Aa1@aaab");

        Assert.IsFalse(claveEsCorrecta);
    }
}
