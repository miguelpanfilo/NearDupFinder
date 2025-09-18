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
}