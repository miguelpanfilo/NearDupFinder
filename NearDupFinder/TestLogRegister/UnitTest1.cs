using System.Security.Claims;
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
    }

    [TestMethod]
    public void RegistrarLog_DeberiaConcatenarDescripcionYDetalles()
    {

        string detalles = "Acción de prueba";

        _sistema.RegistrarLog(AccionLog.AltaUsuario, detalles);

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual("Creacion de usuario: Acción de prueba", logs[0].Detalles);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAsignarAccionCorrectamente()
    {
        string detalles = "Prueba acción";
        _sistema.RegistrarLog(AccionLog.EditarUsuario, detalles);

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual(AccionLog.EditarUsuario, logs[0].Accion);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAsignarUsuarioPredeterminado()
    {
        string detalles = "Prueba usuario";
        _sistema.RegistrarLog(AccionLog.AltaItem, detalles);

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAgregarMultiplesLogs()
    {
        _sistema.RegistrarLog(AccionLog.AltaUsuario, "Primero");
        _sistema.RegistrarLog(AccionLog.EditarUsuario, "Segundo");

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual(2, logs.Count);
        Assert.AreEqual("Creacion de usuario: Primero", logs[0].Detalles);
        Assert.AreEqual("Modificacion de usuario: Segundo", logs[1].Detalles);
    }

    [TestMethod]
    public void RegistrarLog_LogsSeOrdenanPorTiempo()
    {
        _sistema.RegistrarLog(AccionLog.AltaUsuario, "Primero");
        _sistema.RegistrarLog(AccionLog.AltaItem, "Segundo");

        var logs = _sistema.ObtenerLogs();
        Assert.IsTrue(logs[0].Timestamp < logs[1].Timestamp, "El primer log debe ser anterior al segundo");
    }

    [TestMethod]
    public void RegistrarLog_DetallesNoPuedeSerNull()
    {
        _sistema.RegistrarLog(AccionLog.AltaItem, null);

        var logs = _sistema.ObtenerLogs();
        Assert.AreEqual("Alta de item: ", logs[0].Detalles);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaUsarUsuarioActual()
    {
        _sistema.SetUsuarioActual("nuevo@correo.com");

        _sistema.RegistrarLog(AccionLog.AltaUsuario, "Prueba de log");

        var logs = _sistema.ObtenerLogs();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("nuevo@correo.com", logs[0].Usuario);
    }
    
    [TestMethod]
    public void RegistrarLog_DeberiaUsarUsuarioPredeterminadoPorDefecto()
    {
        var sistema = new Sistema();

        sistema.RegistrarLog(AccionLog.AltaUsuario, "Sin usuario seteado");

        var logs = sistema.ObtenerLogs();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }

    [TestMethod]
    public void LogoutUsuario_DeberiaRestablecerUsuarioPredeterminado()
    {
       _sistema.SetUsuarioActual("otro@correo.com");
        _sistema.LogoutUsuario();

        _sistema.RegistrarLog(AccionLog.EditarUsuario, "Logout");

        var logs = _sistema.ObtenerLogs();

        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }
    [TestMethod]
    public void RegistrarLog_DeberiaUsarUltimoUsuarioSeteado()
    {
        _sistema.SetUsuarioActual("primero@correo.com");
        _sistema.SetUsuarioActual("segundo@correo.com");

        _sistema.RegistrarLog(AccionLog.AltaUsuario, "Cambio de usuario");

        var logs = _sistema.ObtenerLogs();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("segundo@correo.com", logs[0].Usuario);
    }

  

}