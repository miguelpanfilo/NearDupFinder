using NearDupFinder_Dominio.Clases;

namespace TestLogRegister;

[TestClass]
public class UnitTest1
{
    private Sistema _sistema;

    [TestInitialize]
    public void Setup()
    {
        _sistema = new Sistema();
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAgregarLogConUsuarioPredeterminado()
    {
        string detalles = "Acción de prueba";

        _sistema.RegistrarLog(AccionLog.AltaUsuario, detalles);

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("test@test.com", logs[0].Usuario);
    }
}