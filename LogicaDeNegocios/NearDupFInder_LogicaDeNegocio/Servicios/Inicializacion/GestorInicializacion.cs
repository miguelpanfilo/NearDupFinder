using NearDupFinder_Dominio.Clases;
using NearDupFinder_Interfaces;

namespace NearDupFInder_LogicaDeNegocio.Servicios.Inicializacion;

public class GestorInicializacion
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IRepositorioSincronizacionIds _repositorioSincronizacion;
    private bool _inicializado;
    private const string EmailAdmin = "admin@gmail.com";

    public GestorInicializacion(
        IRepositorioUsuarios repositorioUsuarios,
        IRepositorioSincronizacionIds repositorioSincronizacion)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _repositorioSincronizacion = repositorioSincronizacion;
    }

    public void AsegurarInicializacion()
    {
        if (_inicializado)
            return;

        AsegurarInicializacionIds();

        if (_repositorioUsuarios.ObtenerUsuarioPorEmail(EmailAdmin) is null)
            CrearUsuarioAdmin();

        _inicializado = true;
    }

    private void AsegurarInicializacionIds()
    {
        int idMaximoUsuario = _repositorioUsuarios.ObtenerIdMaximo();
        Usuario.InicializarGeneradorIds(idMaximoUsuario + 1);

        int idMaximoItem = _repositorioSincronizacion.ObtenerMaximoIdItems();
        Item.ResetearContadorId(idMaximoItem + 1);

        int idMaximoCatalogo = _repositorioSincronizacion.ObtenerMaximoIdCatalogos();
        Catalogo.ResetearContadorIdDesde(idMaximoCatalogo + 1);

        int idMaximoCluster = _repositorioSincronizacion.ObtenerMaximoIdCluster();
        Catalogo.ResetearContadorIdClusterDesde(idMaximoCluster + 1);
    }

    private void CrearUsuarioAdmin()
    {
        Email email = Email.Crear(EmailAdmin);
        Fecha fecha = Fecha.Crear(1997, 12, 27);
        Clave clave = Clave.Crear("123QWEasdzxc@");
        Usuario adminUsuario = Usuario.Crear("admin", "admin", email, fecha);
        adminUsuario.AgregarRol(Rol.Administrador);
        adminUsuario.CambiarClave(clave);
        _repositorioUsuarios.Agregar(adminUsuario);
    }
}