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
}