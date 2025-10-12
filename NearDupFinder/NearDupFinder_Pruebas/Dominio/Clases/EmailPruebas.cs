using NearDupFinder_Dominio.Clases;
using NearDupFinder_Dominio.Excepciones;

namespace NearDupFinder_Pruebas.Dominio.Clases;

[TestClass]
public class EmailPruebas
{
    [TestMethod]
    public void CrearEmail_Nulo_LanzaUsuarioException()
    {
        string? entradaNula = null;
            
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaNula));
    }
    
    [TestMethod]
    public void CrearEmail_Vacio_LanzaUsuarioException()
    {
        string entradaVacia = string.Empty;
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaVacia));
    }

    [TestMethod]
    public void CrearEmal_EspacioEnBlanco_LanzaUsuarioException()
    {
        string entradaEspacioEnBlanco = " ";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaEspacioEnBlanco));
    }

    [TestMethod]
    public void CrearEmail_SinDominio_LanzaUsuarioException()
    {
        string entradaSinDominio = "usuario@";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaSinDominio));
    }

    [TestMethod]
    public void CrearEmail_SoloArrobaYDominio_LanzaUsuarioException()
    {
        string entradaSoloArrobaYDominio = "@ejemplo.com";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaSoloArrobaYDominio));
    }

    [TestMethod]
    public void CrearEmail_SinArroba_LanzaUsuarioException()
    {
        string entradaSinArroba = "usuario.com";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaSinArroba));
    }

    [TestMethod]
    public void CrearEmail_SinPuntoCom_LanzaUsuarioException()
    {
        string entradaSinPuntoCom = "usuario@";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaSinPuntoCom));
    }

    [TestMethod]
    public void CrearEmail_DominioEmpiezaConPunto_LanzaUsuarioException()
    {
        string entradaDominioEmpiezaConPunto = "usuarioo@.com";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaDominioEmpiezaConPunto));
    }

    [TestMethod]
    public void CrearEmail_DominioTerminaConPunto_LanzaUsuarioException()
    {
        string entradaDominioTerminaConPunto = "usuario@ejemplo.";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaDominioTerminaConPunto));
    }
    
    [TestMethod]
    public void CrearEmail_DominioSinPunto_LanzaUsuarioException()
    {
        string entradaDominioSinPunto = "usuario@ejemplocom";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaDominioSinPunto));
    }

    [TestMethod]
    public void CrearEmail_DominioConPuntosConsecutivos_LanzaUsuarioException()
    {
        string entradaDominioConPuntosConsecutivos = "usuario@ejemplo..com";
        
        Assert.ThrowsException<UsuarioException>(() => Email.Crear(entradaDominioConPuntosConsecutivos));
    }

    [TestMethod]
    public void CrearEmail_MayusculasYEspacios_RetornaEmailNormalizado()
    {
        var email = Email.Crear("  Usuario@Ejemplo.Com  ");
        
        Assert.AreEqual("usuario@ejemplo.com", email.ToString());
    }
    
    [TestMethod]
    public void CrearEmail_YaNormalizado_RetornaInstanciaNormalizada()
    {
        var email = Email.Crear("usuario@ejemplo.com");
        
        Assert.AreEqual("usuario@ejemplo.com", email.ToString());
    }

    [TestMethod]
    public void Igual_EmailsEquivalentes_RetornaVerdadero()
    {
        var email1 = Email.Crear("USER@Example.COM");
        var email2 = Email.Crear("user@example.com");
        
        Assert.IsTrue(email1.Igual(email2));
    }

    [TestMethod]
    public void Igual_EmailsDistintos_RetornaFalso()
    {
        var email1 = Email.Crear("user@example.com");
        var email2 = Email.Crear("otro@example.com");
        
        Assert.IsFalse(email1.Igual(email2));
    }
}
