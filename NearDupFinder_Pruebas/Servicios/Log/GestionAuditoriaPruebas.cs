using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFinder_Pruebas.Utilidades;

namespace NearDupFinder_Pruebas.Dominio.Log;
[TestClass]
public class GestionAuditoriaPruebas
{
    private SqlContext _contexto = null!;
    private RepositorioAuditorias _repositorio = null!;
    private GestorAuditoria _gestorAuditoria = null!;
    private SesionUsuarioActual _sesion = null!;

    [TestInitialize]
    public void Setup()
    {
        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BaseDeDatosDePrueba_Auditoria");
        _contexto = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_contexto);

        _repositorio = new RepositorioAuditorias(_contexto);

        _sesion = new SesionUsuarioActual();
        _sesion.Asignar("tester@correo.com");

        _gestorAuditoria = new GestorAuditoria(_repositorio, _sesion);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAgregarLog_Correcto()
    { 
        string detalles = "Acción de prueba";

        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, detalles);

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades();
        Assert.AreEqual(1, logs.Count(), "Debería haberse registrado un log.");

    }

    [TestMethod]
    public void RegistrarLog_DeberiaTenerLosDetalles_Correcto()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "prueba");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual("Creación de usuario: prueba", logs[0].Detalles);
    }

    
    [TestMethod]
    public void RegistrarLog_SinUsuarioAsignado_DeberiaUsarValorPorDefecto()
    {
        var sesion = new SesionUsuarioActual(); 
        var gestor = new GestorAuditoria(_repositorio, sesion);

        gestor.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "prueba");

        var logs = gestor.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }


    [TestMethod]
    public void RegistrarLog_ConUsuarioAsignado_DeberiaRegistrarElUsuario()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Acción de prueba");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual("tester@correo.com", logs[0].Usuario);
    }

    [TestMethod]
    public void RegistrarLog_DeberiaAsignarAccionCorrectamente()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Prueba acción");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual(EntradaDeLog.AccionLog.EditarUsuario, logs[0].Accion);
    }


    [TestMethod]
    public void RegistrarLog_AgregarMultiplesLogs_Correcto()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Primero");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Segundo");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual(2, logs.Count, "Deberían haberse registrado 2 logs.");
        Assert.AreEqual("Creación de usuario: Primero", logs[0].Detalles);
        Assert.AreEqual("Modificación de usuario: Segundo", logs[1].Detalles);
    }

    
    [TestMethod]
    public void RegistrarLog_LogsSeOrdenanPorTiempo()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Primero");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaItem, "Segundo");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.IsTrue(logs[0].Timestamp < logs[1].Timestamp, 
            "El primer log debe ser anterior al segundo.");
    }


    [TestMethod]
    public void RegistrarLog_DetallesNoPuedeSerNull()
    {
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaItem, null);

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual("Alta de ítem: ", logs[0].Detalles);
    }


    [TestMethod]
    public void RegistrarLog_DeberiaUsarUsuarioActual()
    {
        _gestorAuditoria.AsignarUsuarioActual("nuevo@correo.com");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Prueba de log");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual("nuevo@correo.com", logs[0].Usuario);
    }

    
    [TestMethod]
    public void LogoutUsuario_DeberiaRestablecerUsuarioPredeterminado()
    {
        _gestorAuditoria.AsignarUsuarioActual("otro@correo.com");
        _gestorAuditoria.DesasignarUsuario();
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.EditarUsuario, "Logout");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual("No hay usuario logueado", logs[0].Usuario);
    }

    
    [TestMethod]
    public void RegistrarLog_DeberiaUsarUltimoUsuarioSeteado_Correcto()
    {
        _gestorAuditoria.AsignarUsuarioActual("primero@correo.com");
        _gestorAuditoria.AsignarUsuarioActual("segundo@correo.com");
        _gestorAuditoria.RegistrarLog(EntradaDeLog.AccionLog.AltaUsuario, "Cambio de usuario");

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

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

        var logs = _gestorAuditoria.ObtenerTodasLasEntidades().ToList();

        Assert.AreEqual(1, logs.Count, "Debe haberse agregado exactamente un log manual.");
        var log = logs.First();

        Assert.AreEqual(fechaEsperada, log.Timestamp, "La fecha registrada no coincide.");
        Assert.AreEqual(usuarioEsperado, log.Usuario, "El usuario registrado no coincide.");
        Assert.AreEqual(accionEsperada, log.Accion, "La acción registrada no coincide.");
        Assert.AreEqual(detallesEsperados, log.Detalles, "Los detalles registrados no coinciden.");
    }
    
}
