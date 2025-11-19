using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Dominio.Clases;
using NearDupFinder_Pruebas.Utilidades;

namespace NearDupFinder_Pruebas.Repositorios;

[TestClass]
public class RepositorioNotificacionesPruebas
{
    private SqlContext _context = null!;
    private RepositorioNotificaciones _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var opciones = SqlContextFactoryPruebas.CrearOpcionesInMemory("BD_Notificaciones");
        _context = SqlContextFactoryPruebas.CrearContexto(opciones);
        SqlContextFactoryPruebas.LimpiarBaseDeDatos(_context);
        _repo = new RepositorioNotificaciones(_context);
    }

    [TestMethod]
    public void ObtenerPorId_Existente_Ok()
    {
        var n = new Notificacion("mail@test.com", "msg");
        _repo.Agregar(n);
        _repo.GuardarCambios();

        var obtenido = _repo.ObtenerPorId(n.Id);

        Assert.IsNotNull(obtenido);
        Assert.AreEqual(n.Id, obtenido.Id);
    }

    [TestMethod]
    public void ObtenerPorId_Inexistente_RetornaNull()
    {
        var obtenido = _repo.ObtenerPorId(999);
        Assert.IsNull(obtenido);
    }

    [TestMethod]
    public void ObtenerPorEmail_RetornaSoloLasDelUsuario()
    {
        var n1 = new Notificacion("user1@mail.com", "msg1");
        var n2 = new Notificacion("user2@mail.com", "msg2");
        var n3 = new Notificacion("user1@mail.com", "msg3");

        _repo.Agregar(n1);
        _repo.Agregar(n2);
        _repo.Agregar(n3);
        _repo.GuardarCambios();

        var lista = _repo.ObtenerPorEmail("user1@mail.com");

        Assert.AreEqual(2, lista.Count);
        Assert.IsTrue(lista.Any(n => n.Mensaje == "msg1"));
        Assert.IsTrue(lista.Any(n => n.Mensaje == "msg3"));
    }
}