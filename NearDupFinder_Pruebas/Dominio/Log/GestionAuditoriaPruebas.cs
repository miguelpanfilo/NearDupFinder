using NearDupFinder_Dominio.Clases;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;

namespace NearDupFinder_Pruebas.Dominio.Log;

[TestClass]
public class GestionAuditoriaPruebas
{
    private GestorAuditoria _gestorAuditoria = null!;

    [TestInitialize]
    public void Setup()
    {
        _gestorAuditoria = new GestorAuditoria();
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAgregarLog_Correcto()
    { 
        string detalles = "Acción de prueba";

        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, detalles);

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual(1, logs.Count, "Debería haberse registrado un log.");
    }

    [TestMethod]
    public void RegistrarLog_DeberiaTenerLosDetalles_Correcto()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Acción de prueba");

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual("Creacion de usuario: Acción de prueba", logs[0].Detalles);
    }
    
    [TestMethod]
    public void RegistrarLog_DeberiaTenerUsuarioPorDefecto_Correcto()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Acción de prueba");

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario, 
            "El usuario por defecto debe ser 'Anónimo'.");
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAsignarAccionCorrectamente()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Prueba acción");

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual(EntradaDeLog.AccionLog.EditarUsuario, logs[0].Accion);
    }

    [TestMethod]
    public void RegistrarLog_AgregarMultiplesLogs_Correcto()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Primero");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Segundo");

        var logs = _gestorAuditoria.ObtenerLogs();

        Assert.AreEqual(2, logs.Count, "Deberían haberse registrado 2 logs.");
        Assert.AreEqual("Creacion de usuario: Primero", logs[0].Detalles);
        Assert.AreEqual("Modificacion de usuario: Segundo", logs[1].Detalles);
    }
    
    [TestMethod]
    public void RegistrarLog_LogsSeOrdenanPorTiempo()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Primero");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaItem, "Segundo");

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.IsTrue(logs[0].Timestamp < logs[1].Timestamp, 
            "El primer log debe ser anterior al segundo.");
    }

    [TestMethod]
    public void RegistrarLog_DetallesNoPuedeSerNull()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaItem, null);

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual("Alta de item: ", logs[0].Detalles);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaUsarUsuarioActual()
    {
        _gestorAuditoria.AsignarUsuarioActual("nuevo@correo.com");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Prueba de log");

        var logs = _gestorAuditoria.ObtenerLogs();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("nuevo@correo.com", logs[0].Usuario);
    }
    
    [TestMethod]
    public void LogoutUsuario_DeberiaRestablecerUsuarioPredeterminado()
    {
        _gestorAuditoria.AsignarUsuarioActual("otro@correo.com");
        _gestorAuditoria.DesasignarUsuario();
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Logout");

        var logs = _gestorAuditoria.ObtenerLogs();
        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }
    
    [TestMethod]
    public void RegistrarLog_DeberiaUsarUltimoUsuarioSeteado_Correcto()
    {
        _gestorAuditoria.AsignarUsuarioActual("primero@correo.com");
        _gestorAuditoria.AsignarUsuarioActual("segundo@correo.com");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Cambio de usuario");

        var logs = _gestorAuditoria.ObtenerLogs();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("segundo@correo.com", logs[0].Usuario);
    }
    
    [TestMethod]
    public void RegistrarLogManual_DeberiaRegistrarConFechaYUsuarioEspecificos()
    {
        
        var fechaEsperada = new DateTime(2025, 10, 27, 15, 45, 0);
        var usuarioEsperado = "manual@correo.com";
        var accionEsperada = EntradaDeLog.AccionLog.FusionarCluster;
        var detallesEsperados = "Test manual";

        _gestorAuditoria.RegistrarLogManual(fechaEsperada, usuarioEsperado, accionEsperada, detallesEsperados);
        var logs = _gestorAuditoria.ObtenerLogs();

        Assert.AreEqual(1, logs.Count, "Debe haberse agregado exactamente un log manual.");
        var log = logs.First();

        Assert.AreEqual(fechaEsperada, log.Timestamp, "La fecha registrada no coincide.");
        Assert.AreEqual(usuarioEsperado, log.Usuario, "El usuario registrado no coincide.");
        Assert.AreEqual(accionEsperada, log.Accion, "La acción registrada no coincide.");
        Assert.AreEqual(detallesEsperados, log.Detalles, "Los detalles registrados no coinciden.");
    }

    
}
