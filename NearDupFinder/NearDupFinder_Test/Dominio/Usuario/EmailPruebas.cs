using NearDupFinder_Dominio.Usuario;

namespace NearDupFinder_Test.Usuario.Dominio.VO;

[TestClass]
public class EmailPruebas
{
    [TestMethod]
    public void Crear_Valido_DevuelveInstancia()
    {
        // Preparación
        string emailCorrecto = "usuario@ejemplo.com";
        string emailEsperado = "usuario@ejemplo.com";

        // Acción
        var email = Email.Crear(emailCorrecto);

        // Afirmación
        Assert.IsNotNull(email);
        Assert.AreEqual(emailEsperado, email.ToString());
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("usuario@")]
    [DataRow("@ejemplo.com")]
    [DataRow("usuarioejemplo.com")]
    [DataRow("usuario@ejemplo")]
    [DataRow("usuario@.com")]
    [DataRow("usuario@ejemplo.")]
    [DataRow("usuario@ejemplo..com")]
    public void Crear_Invalido_LanzaExcepcion(string textoCorreoInvalido)
    {
        Assert.ThrowsException<ArgumentException>(() => Email.Crear(textoCorreoInvalido));
    }
    
    [DataTestMethod]
    [DataRow("  Usuario@Ejemplo.Com  ", "usuario@ejemplo.com")]
    [DataRow("UsEr.Name+Tag@Sub.Dominio.IO ", "user.name+tag@sub.dominio.io")]
    public void Crear_RecortaEspaciosYNormalizaMinusculas(string correoConEspaciosYMayusculas, string correoNormalizadoEsperado)
    {
        var email = Email.Crear(correoConEspaciosYMayusculas);
        Assert.AreEqual(correoNormalizadoEsperado, email.ToString());
    }
    
    [TestMethod]
    public void Equals_Valido()
    {
        var email1 = Email.Crear("USER@Example.COM");
        var email2 = Email.Crear("user@example.com");

        Assert.IsTrue(email1.Igual(email2));
    }

    [TestMethod]
    public void Equals_Invalido()
    {
        var email1 = Email.Crear("user@example.com");
        var email2 = Email.Crear("otro@example.com");

        Assert.IsFalse(email1.Igual(email2));
    }
}